using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Extensions;
using AlirezaRezaee.Web.Helpers;
using AlirezaRezaee.Web.Helpers.Enums;
using AlirezaRezaee.Web.Models;
using AlirezaRezaee.Web.Models.ViewModels.Posts;
using Markdig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlirezaRezaee.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileManager _ifileManager;
        private readonly IWebHostEnvironment _env;

        public PostsController(ApplicationDbContext context, IFileManager ifileManager, IWebHostEnvironment env)
        {
            _context = context;
            _ifileManager = ifileManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var posts = new List<PostSummaryViewModel>();

            //Todo: performance problem -> not use 2 include for all
            foreach (var post in await _context.Posts.Include(p => p.Article).Include(p => p.Share).Include(p => p.Markdown).OrderByDescending(p => p.PublishDateTime).Take(8).ToListAsync())
            {
                var postType = DetectPostType(post);
                switch (postType)
                {
                    case PostType.Article:
                        posts.Add(new PostSummaryViewModel
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Type = PostType.Article,
                            PublishDateTime = post.PublishDateTime,
                            LatestUpdateDateTime = post.LatestUpdateDateTime,
                            Summary = post.Summary,
                            ThumbnailUrl = post.ThumbnailUrl,
                            PostUrl = post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}",
                            PostEditUrl = "/edit/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}",
                            postDeleteUrl = "/delete/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}",
                            postEditTypeUrl = "/edit/type/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}"
                        });
                        break;
                    case PostType.Share:
                        posts.Add(new PostSummaryViewModel
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Type = PostType.Share,
                            PublishDateTime = post.PublishDateTime,
                            LatestUpdateDateTime = post.LatestUpdateDateTime,
                            Summary = post.Summary,
                            ThumbnailUrl = post.ThumbnailUrl,
                            PostUrl = post.Share.RedirectToUrl,
                            PostEditUrl = "/edit/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + post.Id,
                            postDeleteUrl = "/delete/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + post.Id,
                            postEditTypeUrl = "/edit/type/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + post.Id
                        });
                        break;
                    case PostType.Markdown:
                        posts.Add(new PostSummaryViewModel
                        {
                            Id = post.Id,
                            Title = post.Title,
                            Type = PostType.Markdown,
                            PublishDateTime = post.PublishDateTime,
                            LatestUpdateDateTime = post.LatestUpdateDateTime,
                            Summary = post.Summary,
                            ThumbnailUrl = post.ThumbnailUrl,
                            PostUrl = post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                            PostEditUrl = "/edit/markdown/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                            postDeleteUrl = "/delete/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                            postEditTypeUrl = "/edit/type/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}"
                        });
                        break;
                    default:
                        continue; //نباید تحت هیچ عنوان به اینجا وارد بشه، باید تدابیری اندیشه بشه که اگر چنین اتفاقی افتاد مدیر سایت باخبر بشه، از طریق ایمیل یا لاگ انداختن
                }
            }
            return View(posts);
        }

        private async Task<PostType> DetectPostType(int postId)
        {
            var post = await _context.Posts.Where(p => p.Id == postId).Select(p => new { p.Article, p.Share, p.Markdown }).FirstOrDefaultAsync();

            if (post == null)
                throw new Exception("در مرحله تشخیص نوع مطلب، نوعی برای مطلب یافت نشد.");

            if ((post.Article == null && post.Share == null && post.Markdown == null) || (post.Article != null && post.Share != null && post.Markdown != null))
                throw new Exception("در مرحله تشخیص نوع مطلب، بیش از یک نوع برای مطلب یافت شد.");

            if (post.Article != null)
                return PostType.Article;
            else if (post.Share != null)
                return PostType.Share;
            else //if (post.Markdown != null)
                return PostType.Markdown;
        }

        private PostType? DetectPostType(Post post)
        {
            if (post == null)
                return null;

            if ((post.Article == null && post.Share == null && post.Markdown == null) || (post.Article != null && post.Share != null && post.Markdown != null))
                return null;

            if (post.Article != null)
                return PostType.Article;
            else if (post.Share != null)
                return PostType.Share;
            else //if (post.Markdown != null)
                return PostType.Markdown;
        }

        private string ValidateName(string Name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                Name = Name.Replace(c, ' ');
            return Regex.Replace(Name, @"\s+", "-");
        }

        [Route("{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> Details(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();
            try
            {
                var post = await _context.Posts.Include(p => p.Article).Include(p => p.Share).Include(p => p.Markdown).Where(p => dateTime.Date == p.PublishDateTime.Date && postId == p.Id).FirstOrDefaultAsync();
                if (post == null)
                    return NotFound();

                var postType = DetectPostType(post);

                if (postType == PostType.Article)
                    return View(new DetailArticlePostViewModel
                    {
                        Post = post,
                        PostDetailUrl = post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}",
                        PostEditUrl = "/edit/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}",
                        PostDeleteUrl = "/delete/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}",
                        PostEditTypeUrl = "/edit/type/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}"
                    });
                else if (postType == PostType.Share)
                    return Redirect(post.Share.RedirectToUrl);
                else //if (postType == PostType.Markdown)
                {
                    var markdownContent = string.Empty;
                    using (var client = new HttpClient())
                    {
                        // HTTP POST
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                        var response = client.GetAsync(new Uri(post.Markdown.FileUrl)).Result;
                        using (HttpContent content = response.Content)
                        {
                            Task<string> result = content.ReadAsStringAsync();
                            markdownContent = result.Result;
                        }
                    };

                    return View(nameof(DetailMarkdown), new DetailMarkdownPostViewModel
                    {
                        Markdown = post.Markdown,
                        HtmlContent = markdownContent,
                        PostDetailUrl = post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                        PostDeleteUrl = "/delete/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                        PostEditTypeUrl = "/edit/type/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                        PostEditProperties = "/edit/markdown/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                    });                
                }

            }
            catch (Exception)
            {
                throw; //نمایش صفحه خطا
            }
        }

        [HttpGet]
        [Route("md/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> DetailMarkdown(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Include(p => p.Markdown).Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            var markdownContent = string.Empty;

            using (var client = new HttpClient())
            {
                // HTTP POST
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
                var response = client.GetAsync(new Uri(post.Markdown.FileUrl)).Result;
                using (HttpContent content = response.Content)
                {
                    Task<string> result = content.ReadAsStringAsync();
                    markdownContent = result.Result;
                }
            };

            return View(new DetailMarkdownPostViewModel
            {
                Markdown = post.Markdown,
                HtmlContent = Markdig.Markdown.ToHtml(markdownContent),
                PostDeleteUrl = "/delete/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                PostEditTypeUrl = "/edit/type/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
                PostEditProperties = "/edit/markdown/" + post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Markdown.UrlTitle}",
            });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("posts/create/article")]
        public IActionResult CreateArticle()
        {
            return View();
        }

        [Route("posts/create/share")]
        public IActionResult CreateShare()
        {
            return View();
        }

        [Route("posts/create/markdown")]
        public IActionResult CreateMarkdown()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("posts/create/article")]
        public async Task<IActionResult> CreatingArticle(CreateEditArticlePostViewModel createPostVM)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();

                try
                {
                    if (createPostVM.Article.UrlTitle == null)
                        createPostVM.Article.UrlTitle = createPostVM.Post.Title;
                    createPostVM.Article.UrlTitle = ValidateName(createPostVM.Article.UrlTitle);

                    createPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                    createPostVM.CoverImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    var randomNumber = new Random();
                    var imagePath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd");
                    var thumbnailPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{ValidateName(createPostVM.ThumbnailImage.FileName)}";
                    var coverPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{ValidateName(createPostVM.CoverImage.FileName)}";

                    await _ifileManager.SaveFile(createPostVM.ThumbnailImage, thumbnailPath);
                    await _ifileManager.SaveFile(createPostVM.CoverImage, coverPath);

                    createPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    createPostVM.Article.CoverUrl = $"/{coverPath}";

                    createPostVM.Post.PublishDateTime = dateTime;

                    createPostVM.Article.Post = createPostVM.Post;
                    _context.Add(createPostVM.Article);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["PostCreateStatus"] = e.Message;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(CreateArticle), createPostVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("posts/create/share")]
        public async Task<IActionResult> CreatingShare(CreateEditSharePostViewModel createPostVM)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();

                try
                {
                    createPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    var thumbnailPath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd") + $"/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{ValidateName(createPostVM.ThumbnailImage.FileName)}";

                    await _ifileManager.SaveFile(createPostVM.ThumbnailImage, thumbnailPath);

                    createPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";

                    createPostVM.Post.PublishDateTime = dateTime;

                    createPostVM.Share.Post = createPostVM.Post;

                    _context.Add(createPostVM.Share);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["PostCreateStatus"] = e.Message;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(CreateShare), createPostVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("posts/create/markdown")]
        public async Task<IActionResult> CreatingMarkdown(CreateEditMarkdownPostViewModel createPostVM)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();

                try
                {
                    if (createPostVM.Markdown.UrlTitle == null)
                        createPostVM.Markdown.UrlTitle = createPostVM.Post.Title;
                    createPostVM.Markdown.UrlTitle = ValidateName(createPostVM.Markdown.UrlTitle);

                    createPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    var thumbnailPath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd") + $"/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{ValidateName(createPostVM.ThumbnailImage.FileName)}";

                    await _ifileManager.SaveFile(createPostVM.ThumbnailImage, thumbnailPath);

                    createPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";

                    createPostVM.Post.PublishDateTime = dateTime;

                    createPostVM.Markdown.Post = createPostVM.Post;

                    _context.Add(createPostVM.Markdown);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["PostCreateStatus"] = e.Message;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(nameof(CreateShare), createPostVM);
        }

        [HttpGet]
        [Route("edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> Edit(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var postType = await DetectPostType(postId);

            if (postType == PostType.Article)
            {
                var post = await _context.Posts.Where(p => p.Id == postId).Include(p => p.Article).FirstOrDefaultAsync();
                return RedirectToRoute("EditArticle", new { year = year, month = month, day = day, postId = postId });
            }
            else if (postType == PostType.Share)
            {
                var post = await _context.Posts.Where(p => p.Id == postId).Include(p => p.Share).FirstOrDefaultAsync();
                return RedirectToRoute("EditShare", new { year = year, month = month, day = day, postId = postId });
            }
            else
                return NotFound();
        }

        [HttpGet]
        [Route("edit/article/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}", Name = "EditArticle")]
        public async Task<IActionResult> EditArticle(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Include(p => p.Article).Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            if (post.Article == null)
                post.Article = new Article();

            return View(new CreateEditArticlePostViewModel
            {
                Post = post,
                Article = post.Article
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/article/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> EditArticle(CreateEditArticlePostViewModel editPostVM, int year, int month, int day, int postId, string UrlTitle)
        {
            var publishDateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();
            var previousPost = await _context.Posts.Where(p => p.PublishDateTime.Date == publishDateTime && p.Id == postId).Include(p => p.Article).Include(p => p.Share).Include(p => p.Markdown).AsNoTracking().FirstOrDefaultAsync();
            if (previousPost == null)
                return NotFound();
            var previousType = DetectPostType(previousPost);

            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();
                try
                {
                    if (editPostVM.Article.UrlTitle == null)
                        editPostVM.Article.UrlTitle = editPostVM.Post.Title;
                    editPostVM.Article.UrlTitle = ValidateName(editPostVM.Article.UrlTitle);

                    var randomNumber = new Random();
                    var imagePath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd");

                    if (editPostVM.CoverImage != null) editPostVM.CoverImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                    if (editPostVM.ThumbnailImage != null) editPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    if (editPostVM.CoverImage != null)
                    {
                        var coverPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{ValidateName(editPostVM.CoverImage.FileName)}";

                        if (previousType == PostType.Article)
                            _ifileManager.DeleteFile(previousPost.Article.CoverUrl);

                        await _ifileManager.SaveFile(editPostVM.CoverImage, coverPath);
                        editPostVM.Article.CoverUrl = $"/{coverPath}";
                    }
                    else if (previousType == PostType.Share || previousType == PostType.Markdown) throw new Exception("مرحله ذخیره سازی مقاله به دلیل عدم انتخاب تصویر جلد با خطا رو به رو شد.");
                    else editPostVM.Article.CoverUrl = previousPost.Article.CoverUrl;

                    if (editPostVM.ThumbnailImage != null)
                    {
                        var thumbnailPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{ValidateName(editPostVM.ThumbnailImage.FileName)}";
                        _ifileManager.DeleteFile(previousPost.ThumbnailUrl);
                        await _ifileManager.SaveFile(editPostVM.ThumbnailImage, thumbnailPath);
                        editPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    }
                    else editPostVM.Post.ThumbnailUrl = previousPost.ThumbnailUrl;

                    editPostVM.Post.LatestUpdateDateTime = dateTime;
                    editPostVM.Post.Id = postId;
                    editPostVM.Article.Post = editPostVM.Post;

                    if (previousType != PostType.Article)
                    {
                        if (previousType == PostType.Share) await DeleteShare(previousPost.Share);
                        else if (previousType == PostType.Markdown) await DeleteMarkdown(previousPost.Markdown);
                        //other types (except PostType.Share)
                    }
                    else
                    {
                        editPostVM.Article.PostId = postId;
                    }

                    _context.Update(editPostVM.Article);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["PostCreateStatus"] = e.Message;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(editPostVM);
        }

        [HttpGet]
        [Route("edit/share/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}", Name = "EditShare")]
        public async Task<IActionResult> EditShare(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Include(p => p.Share).Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            return View(new CreateEditSharePostViewModel
            {
                Post = post,
                Share = post.Share
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/share/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}")]
        public async Task<IActionResult> EditShare(CreateEditSharePostViewModel editPostVM, int year, int month, int day, int postId)
        {
            var previousPost = await _context.Posts.AsNoTracking().Where(p => p.Id == postId).Include(p => p.Article).Include(p => p.Share).Include(p => p.Markdown).FirstOrDefaultAsync();
            if (previousPost == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();
                try
                {

                    if (editPostVM.ThumbnailImage != null)
                    {
                        editPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var thumbnailPath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd") + $"/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{ValidateName(editPostVM.ThumbnailImage.FileName)}";
                        await _ifileManager.SaveFile(editPostVM.ThumbnailImage, thumbnailPath);
                        editPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    }
                    else editPostVM.Post.ThumbnailUrl = previousPost.ThumbnailUrl;

                    editPostVM.Post.LatestUpdateDateTime = dateTime;
                    editPostVM.Post.Id = postId;
                    editPostVM.Share.Post = editPostVM.Post;

                    var previousType = DetectPostType(previousPost);
                    if (previousType != PostType.Share)
                    {
                        if (previousType == PostType.Article) await DeleteArticle(previousPost.Article);
                        else if (previousType == PostType.Markdown) await DeleteMarkdown(previousPost.Markdown);
                        //other types (except PostType.Share)
                    }
                    else
                    {
                        editPostVM.Share.PostId = postId;
                    }

                    _context.Update(editPostVM.Share);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["PostCreateStatus"] = e.Message;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(editPostVM);
        }

        [HttpGet]
        [Route("edit/markdown/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}", Name = "EditMarkdown")]
        public async Task<IActionResult> EditMarkdown(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Include(p => p.Markdown).Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            return View(new CreateEditMarkdownPostViewModel
            {
                Post = post,
                Markdown = post.Markdown
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/markdown/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> EditMarkdown(CreateEditMarkdownPostViewModel editPostVM, int year, int month, int day, int postId)
        {
            var previousPost = await _context.Posts.AsNoTracking().Where(p => p.Id == postId).Include(p => p.Article).Include(p => p.Share).Include(p => p.Markdown).FirstOrDefaultAsync();
            if (previousPost == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();
                try
                {

                    if (editPostVM.ThumbnailImage != null)
                    {
                        editPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var thumbnailPath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd") + $"/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{ValidateName(editPostVM.ThumbnailImage.FileName)}";
                        await _ifileManager.SaveFile(editPostVM.ThumbnailImage, thumbnailPath);
                        editPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    }
                    else editPostVM.Post.ThumbnailUrl = previousPost.ThumbnailUrl;

                    editPostVM.Post.LatestUpdateDateTime = dateTime;
                    editPostVM.Post.Id = postId;
                    editPostVM.Markdown.Post = editPostVM.Post;

                    var previousType = DetectPostType(previousPost);
                    if (previousType != PostType.Markdown)
                    {
                        if (previousType == PostType.Article) await DeleteArticle(previousPost.Article);
                        else if (previousType == PostType.Share) await DeleteShare(previousPost.Share);
                        //other types (except PostType.Share)
                    }
                    else
                    {
                        editPostVM.Markdown.PostId = postId;
                    }

                    _context.Update(editPostVM.Markdown);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["PostCreateStatus"] = e.Message;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(editPostVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> Delete(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).Include(p => p.Article).Include(p => p.Share).Include(p => p.Markdown).AsNoTracking().FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            var postType = DetectPostType(post);

            try
            {
                await DeletePost(post);
            }
            catch (Exception e)
            {
                TempData["PostDeleteStatus"] = e.Message;
            }

            TempData["PostDeleteStatus"] = "OK";
            return RedirectToAction(nameof(Index));
        }

        private async Task DeleteArticle(Article article)
        {
            _ifileManager.DeleteFile(article.CoverUrl);

            article.Post = null;
            _context.Remove(article);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteShare(Share share)
        {
            share.Post = null;
            _context.Remove(share);
            await _context.SaveChangesAsync();
        }

        private async Task DeleteMarkdown(Models.Markdown markdown)
        {
            markdown.Post = null;
            _context.Remove(markdown);
            await _context.SaveChangesAsync();
        }

        private async Task DeletePost(Post post)
        {
            _ifileManager.DeleteFile(post.ThumbnailUrl);

            if (DetectPostType(post) == PostType.Article) _ifileManager.DeleteFile(post.Article.CoverUrl);

            _context.Remove(post);
            await _context.SaveChangesAsync();
        }

        [Route("edit/type/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> EditType(int year, int month, int day, int postId, string UrlTitle)
        {
            var switchAbleTypes = new List<SwitchPostTypeViewModel>();
            try
            {
                var postType = await DetectPostType(postId);

                if (postType != PostType.Article) switchAbleTypes.Add(new SwitchPostTypeViewModel { PostType = PostType.Article, PostTypeName = "تغییر به مقاله", SwitchUrl = $"/edit/switch/to-article/{year}/{month}/{day}/{postId}" });
                if (postType != PostType.Share) switchAbleTypes.Add(new SwitchPostTypeViewModel { PostType = PostType.Share, PostTypeName = "تغییر به بازنشر", SwitchUrl = $"/edit/switch/to-share/{year}/{month}/{day}/{postId}" });
                if (postType != PostType.Markdown) switchAbleTypes.Add(new SwitchPostTypeViewModel { PostType = PostType.Markdown, PostTypeName = "تغییر به مطلب نشانه دار", SwitchUrl = $"/edit/switch/to-markdown/{year}/{month}/{day}/{postId}" });
            }
            catch (Exception)
            {
                throw; // نمایش صفحهی مخصوص خطا
            }

            return View(switchAbleTypes);
        }

        [Route("edit/switch/to-article/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}")]
        public async Task<IActionResult> SwitchToArticle(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            return RedirectToAction(nameof(EditArticle), new { year = year, month = month, day = day, postId = postId });
        }

        [Route("edit/switch/to-share/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}")]
        public async Task<IActionResult> SwitchToShare(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            return RedirectToAction(nameof(EditShare), new { year = year, month = month, day = day, postId = postId });
        }

        [Route("edit/switch/to-markdown/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}")]
        public async Task<IActionResult> SwitchToMarkdown(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            return RedirectToAction(nameof(EditMarkdown), new { year = year, month = month, day = day, postId = postId });
        }
    }
}