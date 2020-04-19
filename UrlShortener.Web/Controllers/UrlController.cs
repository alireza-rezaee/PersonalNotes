using System;
using System.Collections.Generic;
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
        [Route("")]
        public async Task<IActionResult> Add(UrlCreateEditViewModel createViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!await RecaptchaCheckAsync(createViewModel.Recaptcha)) return BadRequest();

                if (!createViewModel.IsCustomShortLink)
                {
                    var createdShortLink = await Create(createViewModel.Url);
                    if (!string.IsNullOrEmpty(createdShortLink))
                    {
                        ViewData["ShortLink"] = createdShortLink;
                        return View("Result");
                    }
                    else return BadRequest();
                }
                else
                {
                    var createdShortLink = await CreateCustom(createViewModel.Url, createViewModel.CustomShortLink);
                    if (!string.IsNullOrEmpty(createdShortLink))
                    {
                        ViewData["ShortLink"] = createdShortLink;
                        return View("Result");
                    }
                    else return BadRequest();
                }
            }

            return NotFound();
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
                if (!url.StartsWith("https://") && !url.StartsWith("https://"))
                    url = $"http://{url}";
                if (!IsValidUri(url))
                    return null;

                var scheme = GetProtocol(url);
                url = RemoveUrlScheme(url);
                var protocol = await _context.Protocols.FirstOrDefaultAsync(p => p.ProtocolName == scheme);
                if (protocol == null)
                    return null;

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

                throw;
            }
        }

        private async Task<string> CreateCustom(string url, string shortLink)
        {
            try
            {
                if (!url.StartsWith("https://") && !url.StartsWith("https://"))
                    url = $"http://{url}";
                if (!IsValidUri(url))
                    return null;

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

                throw;
            }
        }

        private async Task<string> GetShortLink(string url, string scheme)
        {
            var existingUrl = await _context.Urls.Include(u => u.Protocol).Where(u => u.Location == url && u.Protocol.ProtocolName == scheme).Select(u => new { u.Id }).FirstOrDefaultAsync();
            if (existingUrl != null)
                return Transducer.Encode(existingUrl.Id);
            return null;
        }

        private async Task<string> GetCustomShortLink(string url, string scheme)
        {
            var shortLink = await _context.CustomUrls.Include(u => u.Protocol).Where(u => u.Location == url && u.Protocol.ProtocolName == scheme).Select(u => u.CostomShortLink).FirstOrDefaultAsync();
            if (shortLink != null)
                return shortLink;
            return null;
        }

        private async Task<string> GetCustomUrl(string shortlink) => await _context.CustomUrls.Include(u => u.Protocol).Where(u => u.CostomShortLink == shortlink)
            .Select(u => $"{u.Protocol.ProtocolName}://{u.Location}").FirstOrDefaultAsync();

        private async Task<string> GetUrl(int id) =>
             await _context.Urls.Include(u => u.Protocol).Where(u => u.Id == id).Select(u => $"{u.Protocol.ProtocolName}://{u.Location}").FirstOrDefaultAsync();

        private bool IsValidUri(string uri) => Uri.IsWellFormedUriString(uri, UriKind.Absolute);

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
            catch (Exception)
            {
                return false;
            }
        }
    }
}
