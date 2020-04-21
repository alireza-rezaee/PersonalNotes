using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Web.Data;
using UrlShortener.Web.Helpers;
using UrlShortener.Web.Models;
using UrlShortener.Web.Models.ViewModels;

namespace UrlShortener.Web.Controllers
{
    public class UrlController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UrlController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UrlCreateEditViewModel createViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    await RecaptchaCheckAsync(createViewModel.Recaptcha);

                    if (!createViewModel.IsCustomShortLink)
                    {
                        var createdShortLink = await Create(createViewModel.Url);
                        if (!string.IsNullOrEmpty(createdShortLink))
                        {
                            ViewData["ShortLink"] = createdShortLink;
                            return View("Result");
                        }
                    }
                    else
                    {
                        var createdShortLink = await CreateCustom(createViewModel.Url, createViewModel.CustomShortLink);
                        if (!string.IsNullOrEmpty(createdShortLink))
                        {
                            ViewData["ShortLink"] = createdShortLink;
                            return View("Result");
                        }
                    }
                }
                catch (Exception e)
                {
                    TempData["Error"] = e.Message;
                    throw e;
                }
            }

            return View(createViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddApi(string url, string shortLink, string secretKey)
        {
            if (secretKey != "3cbzRnBKJu7dryLKQSftUvcqwBDtNEKxNtZ5RBw2u46zEjNM2Z23xm4AnccDPaJB")
                return NotFound();

            if (string.IsNullOrEmpty(shortLink))
            {
                var createdShortLink = await Create(url);
                if (!string.IsNullOrEmpty(createdShortLink)) return Content(createdShortLink, "text/plain", System.Text.Encoding.UTF8);
                else return BadRequest();
            }
            else
            {
                var createdShortLink = await CreateCustom(url, shortLink);
                if (!string.IsNullOrEmpty(createdShortLink)) return Content(createdShortLink, "text/plain", System.Text.Encoding.UTF8);
                else return BadRequest();
            }

        }

        [Route("{shortlink}")]
        public async Task<IActionResult> VisitUrl(string shortlink)
        {
            foreach (var c in shortlink)
                if (!Transducer.Alphabet.Contains(c)) return BadRequest();

            string url = string.Empty;

            url = await GetUrl(Transducer.Decode(shortlink));
            if (string.IsNullOrEmpty(url))
                url = await GetCustomUrl(shortlink);

            if (!string.IsNullOrEmpty(url))
                return Redirect(url);

            return NotFound();
        }

        //Private:

        private async Task<string> Create(string url)
        {
            try
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                    url = $"http://{url}";
                if (!IsValidUri(url))
                    throw new Exception("عبارت وارد شده به عنوان نشانی پشتیبانی نمی شود.");
                await IsAvailableUrl(url);

                var scheme = GetProtocol(url);
                url = RemoveUrlScheme(url);
                var protocol = await _context.Protocols.FirstOrDefaultAsync(p => p.ProtocolName == scheme);
                if (protocol == null)
                    throw new Exception("پروتکل نشانی وارد شده هنوز پشتیبانی نمی شود.");

                var shortLink = await GetCustomShortLink(url, scheme);
                if (shortLink != null)
                    return shortLink;
                shortLink = await GetShortLink(url, scheme);
                if (shortLink != null)
                    return shortLink;

                var urlModel = new Url
                {
                    IsEnabled = true,
                    Location = url,
                    ProtocolId = protocol.Id
                };

                _context.Update(urlModel);
                await _context.SaveChangesAsync();

                return await GetShortLink(url, scheme);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                throw e;
            }
        }

        private async Task<string> CreateCustom(string url, string shortLink)
        {
            try
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                    url = $"http://{url}";
                if (!IsValidUri(url))
                    return null;
                await IsAvailableUrl(url);

                var scheme = GetProtocol(url);
                url = RemoveUrlScheme(url);
                var protocol = await _context.Protocols.FirstOrDefaultAsync(p => p.ProtocolName == scheme);
                if (protocol == null)
                    return null;

                var existingShortLink = await GetCustomShortLink(url, scheme);
                if (existingShortLink != null)
                    return existingShortLink;

                var urlModel = new CustomUrl
                {
                    IsEnabled = true,
                    Location = url,
                    ProtocolId = protocol.Id,
                    CostomShortLink = shortLink
                };

                _context.Update(urlModel);
                await _context.SaveChangesAsync();

                return await GetCustomShortLink(url, scheme);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                throw e;
            }
        }

        private async Task<string> GetShortLink(string url, string scheme)
        {
            try
            {
                var link = await _context.Urls.Include(u => u.Protocol).Where(u => u.Location == url && u.Protocol.ProtocolName == scheme).Select(u => new { u.Id }).FirstOrDefaultAsync();
                if (link == null) return null;
                return Transducer.Encode(link.Id);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                throw e;
            }
        }

        private async Task<string> GetCustomShortLink(string url, string scheme)
        {
            try
            {
                var link = await _context.CustomUrls.Include(u => u.Protocol).Where(u => u.Location == url && u.Protocol.ProtocolName == scheme).Select(u => u.CostomShortLink).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(link)) return null;
                return link;
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                throw e;
            }
        }

        private async Task<string> GetCustomUrl(string shortlink)
        {
            try
            {
                return await _context.CustomUrls.Include(u => u.Protocol).Where(u => u.CostomShortLink == shortlink).Select(u => $"{u.Protocol.ProtocolName}://{u.Location}").FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                throw e;
            }

        }

        private async Task<string> GetUrl(int id)
        {
            try
            {
                return await _context.Urls.Include(u => u.Protocol).Where(u => u.Id == id).Select(u => $"{u.Protocol.ProtocolName}://{u.Location}").FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                throw e;
            }

        }

        private bool IsValidUri(string uri) => Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute);

        private async Task<bool> IsAvailableUrl(string uri)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(string.Empty);
                    var result = await client.PostAsync(uri, content);
                    var status = result.StatusCode;
                    string resultContent = await result.Content.ReadAsStringAsync();
                }
                return true;
            }
            catch (Exception e)
            {
                TempData["Error"] = "<p class=\"mb-2\">در حال حاضر نشانی درخواستی خارج از دسترس ماست.</p><p>پس از بررسی این موارد دوباره تلاش کنید:<p><ul><li><b class=\"font-weight-bold\">درستی نشانی</b> وارد شده را بررسی کنید.</li><li>از مجرمانه یا <b class=\"font-weight-bold\">فیلتر نبودن</b> نبودن سایت مطمئن شوید.</li><li>از <b class=\"font-weight-bold\">تحریم نبودن</b> ایران اطمینان حاصل کنید.</li></ul>";
                throw e;
            }

        }

        private string GetProtocol(string url) => new Uri(url).Scheme;

        private string RemoveUrlScheme(string url) => _ = url.Substring(url.IndexOf("://") + 3);

        private async Task<bool> RecaptchaCheckAsync(string responseToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.google.com");
                    var values = new Dictionary<string, string>
                    {
                        { "secret", "6LfKS-sUAAAAAIDGfLgjE-YfdyvqaWWb12hKz32o" },
                        { "response", responseToken }
                    };
                    var content = new FormUrlEncodedContent(values);
                    var result = await client.PostAsync("/recaptcha/api/siteverify", content);
                    string resultContent = await result.Content.ReadAsStringAsync();

                    var recaptcha = JsonSerializer.Deserialize<Recaptcha>(resultContent);
                    return recaptcha.Success;
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "احراز هویت «من ربات نیستم» تایید نشد.";
                throw e;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
