using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rezaee.Alireza.Web.Data;
using Rezaee.Alireza.Web.Extensions;
using Rezaee.Alireza.Web.Helpers;
using Rezaee.Alireza.Web.Helpers.Enums;
using Rezaee.Alireza.Web.Models;
using Rezaee.Alireza.Web.Models.ViewModels.Posts;
using Markdig;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Rezaee.Alireza.Web.Controllers
{
    [Route("posts")]
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

        [Route("")]
        public async Task<IActionResult> Index() => View(await RetrieveLatestPostsSummary(count: 10));

        [NonAction]
        public async Task<List<PostSummaryViewModel>> LoadPosts(int count, int skip) => await RetrieveLatestPostsSummary(count: count, skip: skip);

        [Route("/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
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
        [Route("/md/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
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
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("create/article")]
        public IActionResult CreateArticle()
        {
            return View();
        }

        [Route("create/share")]
        public IActionResult CreateShare()
        {
            return View();
        }

        [Route("create/markdown")]
        public IActionResult CreateMarkdown()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create/article")]
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
                    createPostVM.Article.UrlTitle = Helpers.File.ValidateName(createPostVM.Article.UrlTitle);

                    createPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                    createPostVM.CoverImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    var randomNumber = new Random();
                    var imagePath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd");
                    var thumbnailPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{Helpers.File.ValidateName(createPostVM.ThumbnailImage.FileName)}";
                    var coverPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{Helpers.File.ValidateName(createPostVM.CoverImage.FileName)}";

                    await _ifileManager.SaveFile(createPostVM.ThumbnailImage, thumbnailPath);
                    await _ifileManager.SaveFile(createPostVM.CoverImage, coverPath);

                    createPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    createPostVM.Article.CoverUrl = $"/{coverPath}";

                    createPostVM.Post.PublishDateTime = dateTime;

                    //< برچسب ها
                    string[] tagsArray = { };
                    if (!string.IsNullOrEmpty(createPostVM.PostTags)) tagsArray = createPostVM.PostTags.Split(",");

                    //get new tags from database
                    var toConnentTags = await _context.Tags.Where(t => tagsArray.Contains(t.Title)).ToListAsync();
                    var doneTags = new List<Tag>();

                    //Connect to new tags (Tags that already existed)
                    foreach (var toConnectTag in toConnentTags)
                        if (!doneTags.Any(doneTag => doneTag.Title == toConnectTag.Title))
                            _context.PostTags.Add(new PostTag { Post = createPostVM.Post, TagId = toConnectTag.Id });

                    //add new tags (tags that didn't exist before)
                    var newTags = new List<Tag>();
                    foreach (var tag in tagsArray)
                        if (!doneTags.Any(doneTag => doneTag.Title == tag) && !toConnentTags.Where(tt => tt.Title == tag).Any())
                            _context.PostTags.Add(new PostTag { Tag = new Tag { Title = tag }, Post = createPostVM.Post });
                    //> برچسب ها

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
        [Route("create/share")]
        public async Task<IActionResult> CreatingShare(CreateEditSharePostViewModel createPostVM)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();

                try
                {
                    createPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    var thumbnailPath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd") + $"/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{Helpers.File.ValidateName(createPostVM.ThumbnailImage.FileName)}";

                    await _ifileManager.SaveFile(createPostVM.ThumbnailImage, thumbnailPath);

                    createPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";

                    createPostVM.Post.PublishDateTime = dateTime;


                    //< برچسب ها
                    string[] tagsArray = { };
                    if (!string.IsNullOrEmpty(createPostVM.PostTags)) tagsArray = createPostVM.PostTags.Split(",");

                    //get new tags from database
                    var toConnentTags = await _context.Tags.Where(t => tagsArray.Contains(t.Title)).ToListAsync();
                    var doneTags = new List<Tag>();

                    //Connect to new tags (Tags that already existed)
                    foreach (var toConnectTag in toConnentTags)
                        if (!doneTags.Any(doneTag => doneTag.Title == toConnectTag.Title))
                            _context.PostTags.Add(new PostTag { Post = createPostVM.Post, TagId = toConnectTag.Id });

                    //add new tags (tags that didn't exist before)
                    var newTags = new List<Tag>();
                    foreach (var tag in tagsArray)
                        if (!doneTags.Any(doneTag => doneTag.Title == tag) && !toConnentTags.Where(tt => tt.Title == tag).Any())
                            _context.PostTags.Add(new PostTag { Tag = new Tag { Title = tag }, Post = createPostVM.Post });
                    //> برچسب ها


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
        [Route("create/markdown")]
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
                    createPostVM.Markdown.UrlTitle = Helpers.File.ValidateName(createPostVM.Markdown.UrlTitle);

                    createPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    var thumbnailPath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd") + $"/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{Helpers.File.ValidateName(createPostVM.ThumbnailImage.FileName)}";

                    await _ifileManager.SaveFile(createPostVM.ThumbnailImage, thumbnailPath);

                    createPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";

                    createPostVM.Post.PublishDateTime = dateTime;


                    //< برچسب ها
                    string[] tagsArray = { };
                    if (!string.IsNullOrEmpty(createPostVM.PostTags)) tagsArray = createPostVM.PostTags.Split(",");

                    //get new tags from database
                    var toConnentTags = await _context.Tags.Where(t => tagsArray.Contains(t.Title)).ToListAsync();
                    var doneTags = new List<Tag>();

                    //Connect to new tags (Tags that already existed)
                    foreach (var toConnectTag in toConnentTags)
                        if (!doneTags.Any(doneTag => doneTag.Title == toConnectTag.Title))
                            _context.PostTags.Add(new PostTag { Post = createPostVM.Post, TagId = toConnectTag.Id });

                    //add new tags (tags that didn't exist before)
                    var newTags = new List<Tag>();
                    foreach (var tag in tagsArray)
                        if (!doneTags.Any(doneTag => doneTag.Title == tag) && !toConnentTags.Where(tt => tt.Title == tag).Any())
                            _context.PostTags.Add(new PostTag { Tag = new Tag { Title = tag }, Post = createPostVM.Post });
                    //> برچسب ها


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
        [Route("/edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
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
        [Route("/edit/article/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}", Name = "EditArticle")]
        public async Task<IActionResult> EditArticle(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Include(p => p.Article).Include(p => p.PostTags).ThenInclude(p => p.Tag).Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            if (post.Article == null)
                post.Article = new Article();

            //< برچسب ها
            var postTags = string.Empty;
            foreach (var postTag in post.PostTags)
                postTags += postTag.Tag.Title + ',';
            if (!string.IsNullOrEmpty(postTags))
                postTags.Substring(0, postTags.Length - 1);
            //> برچسب ها

            return View(new CreateEditArticlePostViewModel
            {
                Post = post,
                Article = post.Article,
                PostTags = postTags
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/edit/article/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> EditArticle(CreateEditArticlePostViewModel editPostVM, int year, int month, int day, int postId, string UrlTitle)
        {
            var publishDateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();
            var previousPost = await _context.Posts.Where(p => p.PublishDateTime.Date == publishDateTime && p.Id == postId).Include(p => p.Article).Include(p => p.Share).Include(p => p.Markdown).Include(p => p.PostTags).ThenInclude(p => p.Tag).AsNoTracking().FirstOrDefaultAsync();
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
                    editPostVM.Article.UrlTitle = Helpers.File.ValidateName(editPostVM.Article.UrlTitle);

                    var randomNumber = new Random();
                    var imagePath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd");

                    if (editPostVM.CoverImage != null) editPostVM.CoverImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                    if (editPostVM.ThumbnailImage != null) editPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    if (editPostVM.CoverImage != null)
                    {
                        var coverPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{Helpers.File.ValidateName(editPostVM.CoverImage.FileName)}";

                        if (previousType == PostType.Article)
                            _ifileManager.DeleteFile(previousPost.Article.CoverUrl);

                        await _ifileManager.SaveFile(editPostVM.CoverImage, coverPath);
                        editPostVM.Article.CoverUrl = $"/{coverPath}";
                    }
                    else if (previousType == PostType.Share || previousType == PostType.Markdown) throw new Exception("مرحله ذخیره سازی مقاله به دلیل عدم انتخاب تصویر جلد با خطا رو به رو شد.");
                    else editPostVM.Article.CoverUrl = previousPost.Article.CoverUrl;

                    if (editPostVM.ThumbnailImage != null)
                    {
                        var thumbnailPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{Helpers.File.ValidateName(editPostVM.ThumbnailImage.FileName)}";
                        _ifileManager.DeleteFile(previousPost.ThumbnailUrl);
                        await _ifileManager.SaveFile(editPostVM.ThumbnailImage, thumbnailPath);
                        editPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    }
                    else editPostVM.Post.ThumbnailUrl = previousPost.ThumbnailUrl;


                    //< برچسب ها
                    string[] tagsArray = { };
                    if (!string.IsNullOrEmpty(editPostVM.PostTags)) tagsArray = editPostVM.PostTags.Split(",");

                    //get new tags from database
                    var toConnentTags = await _context.Tags.Where(t => tagsArray.Contains(t.Title)).AsNoTracking().ToListAsync();
                    var doneTags = new List<Tag>();

                    //remove previous tags (Tags that are no longer relevant)
                    foreach (var previousPostTag in previousPost.PostTags)
                        if (!tagsArray.Contains(previousPostTag.Tag.Title))
                        {
                            previousPostTag.Post = null;
                            _context.PostTags.Remove(previousPostTag);
                        }
                        else doneTags.Add(previousPostTag.Tag);

                    //Connect to new tags (Tags that already existed)
                    foreach (var toConnectTag in toConnentTags)
                        if (!doneTags.Any(doneTag => doneTag.Title == toConnectTag.Title))
                            _context.PostTags.Add(new PostTag { PostId = postId, TagId = toConnectTag.Id });

                    //add new tags (tags that didn't exist before)
                    var newTags = new List<Tag>();
                    foreach (var tag in tagsArray)
                        if (!doneTags.Any(doneTag => doneTag.Title == tag) && !toConnentTags.Where(tt => tt.Title == tag).Any())
                            _context.PostTags.Add(new PostTag { Tag = new Tag { Title = tag }, PostId = postId });
                    //> برچسب ها

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
        [Route("/edit/share/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}", Name = "EditShare")]
        public async Task<IActionResult> EditShare(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Include(p => p.Share).Include(p => p.PostTags).ThenInclude(p => p.Tag).Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            //< برچسب ها
            var postTags = string.Empty;
            foreach (var postTag in post.PostTags)
                postTags += postTag.Tag.Title + ',';
            if (!string.IsNullOrEmpty(postTags))
                postTags.Substring(0, postTags.Length - 1);
            //> برچسب ها

            return View(new CreateEditSharePostViewModel
            {
                Post = post,
                Share = post.Share,
                PostTags = postTags
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/edit/share/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}")]
        public async Task<IActionResult> EditShare(CreateEditSharePostViewModel editPostVM, int year, int month, int day, int postId)
        {
            var previousPost = await _context.Posts.AsNoTracking().Where(p => p.Id == postId).Include(p => p.Article).Include(p => p.PostTags).ThenInclude(p => p.Tag).Include(p => p.Share).Include(p => p.Markdown).FirstOrDefaultAsync();
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
                        var thumbnailPath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd") + $"/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{Helpers.File.ValidateName(editPostVM.ThumbnailImage.FileName)}";
                        await _ifileManager.SaveFile(editPostVM.ThumbnailImage, thumbnailPath);
                        editPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    }
                    else editPostVM.Post.ThumbnailUrl = previousPost.ThumbnailUrl;


                    //< برچسب ها
                    string[] tagsArray = { };
                    if (!string.IsNullOrEmpty(editPostVM.PostTags)) tagsArray = editPostVM.PostTags.Split(",");

                    //get new tags from database
                    var toConnentTags = await _context.Tags.Where(t => tagsArray.Contains(t.Title)).AsNoTracking().ToListAsync();
                    var doneTags = new List<Tag>();

                    //remove previous tags (Tags that are no longer relevant)
                    foreach (var previousPostTag in previousPost.PostTags)
                        if (!tagsArray.Contains(previousPostTag.Tag.Title))
                        {
                            previousPostTag.Post = null;
                            _context.PostTags.Remove(previousPostTag);
                        }
                        else doneTags.Add(previousPostTag.Tag);

                    //Connect to new tags (Tags that already existed)
                    foreach (var toConnectTag in toConnentTags)
                        if (!doneTags.Any(doneTag => doneTag.Title == toConnectTag.Title))
                            _context.PostTags.Add(new PostTag { PostId = postId, TagId = toConnectTag.Id });

                    //add new tags (tags that didn't exist before)
                    var newTags = new List<Tag>();
                    foreach (var tag in tagsArray)
                        if (!doneTags.Any(doneTag => doneTag.Title == tag) && !toConnentTags.Where(tt => tt.Title == tag).Any())
                            _context.PostTags.Add(new PostTag { Tag = new Tag { Title = tag }, PostId = postId });
                    //> برچسب ها

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
        [Route("/edit/markdown/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}", Name = "EditMarkdown")]
        public async Task<IActionResult> EditMarkdown(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Include(p => p.Markdown).Include(p => p.PostTags).ThenInclude(p => p.Tag).Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            //< برچسب ها
            var postTags = string.Empty;
            foreach (var postTag in post.PostTags)
                postTags += postTag.Tag.Title + ',';
            if (!string.IsNullOrEmpty(postTags))
                postTags.Substring(0, postTags.Length - 1);
            //> برچسب ها

            return View(new CreateEditMarkdownPostViewModel
            {
                Post = post,
                Markdown = post.Markdown,
                PostTags = postTags
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/edit/markdown/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> EditMarkdown(CreateEditMarkdownPostViewModel editPostVM, int year, int month, int day, int postId)
        {
            var previousPost = await _context.Posts.AsNoTracking().Where(p => p.Id == postId).Include(p => p.Article).Include(p => p.PostTags).ThenInclude(p => p.Tag).Include(p => p.Share).Include(p => p.Markdown).FirstOrDefaultAsync();
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
                        var thumbnailPath = "uploads/images/" + persianDateTime.ToString("yyyy/MM/dd") + $"/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999)}_{Helpers.File.ValidateName(editPostVM.ThumbnailImage.FileName)}";
                        await _ifileManager.SaveFile(editPostVM.ThumbnailImage, thumbnailPath);
                        editPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    }
                    else editPostVM.Post.ThumbnailUrl = previousPost.ThumbnailUrl;


                    //< برچسب ها
                    string[] tagsArray = { };
                    if (!string.IsNullOrEmpty(editPostVM.PostTags)) tagsArray = editPostVM.PostTags.Split(",");

                    //get new tags from database
                    var toConnentTags = await _context.Tags.Where(t => tagsArray.Contains(t.Title)).AsNoTracking().ToListAsync();
                    var doneTags = new List<Tag>();

                    //remove previous tags (Tags that are no longer relevant)
                    foreach (var previousPostTag in previousPost.PostTags)
                        if (!tagsArray.Contains(previousPostTag.Tag.Title))
                        {
                            previousPostTag.Post = null;
                            _context.PostTags.Remove(previousPostTag);
                        }
                        else doneTags.Add(previousPostTag.Tag);

                    //Connect to new tags (Tags that already existed)
                    foreach (var toConnectTag in toConnentTags)
                        if (!doneTags.Any(doneTag => doneTag.Title == toConnectTag.Title))
                            _context.PostTags.Add(new PostTag { PostId = postId, TagId = toConnectTag.Id });

                    //add new tags (tags that didn't exist before)
                    var newTags = new List<Tag>();
                    foreach (var tag in tagsArray)
                        if (!doneTags.Any(doneTag => doneTag.Title == tag) && !toConnentTags.Where(tt => tt.Title == tag).Any())
                            _context.PostTags.Add(new PostTag { Tag = new Tag { Title = tag }, PostId = postId });
                    //> برچسب ها


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
        [Route("/delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> Delete(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts
                .Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId)
                .Include(p => p.Article)
                .Include(p => p.Share)
                .Include(p => p.Markdown)
                .AsNoTracking()
                .FirstOrDefaultAsync();
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

        [Route("/edit/type/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
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

        [Route("/edit/switch/to-article/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}")]
        public async Task<IActionResult> SwitchToArticle(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            return RedirectToAction(nameof(EditArticle), new { year = year, month = month, day = day, postId = postId });
        }

        [Route("/edit/switch/to-share/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}")]
        public async Task<IActionResult> SwitchToShare(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            return RedirectToAction(nameof(EditShare), new { year = year, month = month, day = day, postId = postId });
        }

        [Route("/edit/switch/to-markdown/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}")]
        public async Task<IActionResult> SwitchToMarkdown(int year, int month, int day, int postId)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Where(p => p.PublishDateTime.Date == dateTime && p.Id == postId).FirstOrDefaultAsync();
            if (post == null)
                return NotFound();

            return RedirectToAction(nameof(EditMarkdown), new { year = year, month = month, day = day, postId = postId });
        }

        private async Task<List<PostSummaryViewModel>> RetrieveLatestPostsSummary(int count, int skip = 0) => await RetrieveLatestPostsSummary(count: count, skip: skip, context: _context);

        public static async Task<List<PostSummaryViewModel>> RetrieveLatestPostsSummary(int count, ApplicationDbContext context, int skip = 0)
        {
            var posts = new List<PostSummaryViewModel>();
            var retrievePosts = await context.Posts
                .OrderByDescending(p => p.Posterpins.Id)
                .ThenByDescending(p => p.Pin.Id)
                .ThenByDescending(p => p.PublishDateTime)
                .Include(p => p.Pin)
                .Include(p => p.Posterpins)
                .Include(p => p.Article)
                .Include(p => p.Share)
                .Include(p => p.Markdown)
                .Include(p => p.DestructivePosts)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag).ThenInclude(t => t.PostTags)
                .Skip(skip)
                .Take(count)
                .ToListAsync();
            await context.Posts.Where(p => RetrievePostIdsForRelatedPosts(retrievePosts).Contains(p.Id))
                .Include(p => p.Article)
                .Include(p => p.Share)
                .Include(p => p.Markdown)
                .Include(p => p.DestructivePosts)
                .ToListAsync();

            //Todo: performance problem -> not use 2 include for all
            foreach (var post in retrievePosts)
            {
                posts.Add(new PostSummaryViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Type = DetectPostType(post),
                    PublishDateTime = post.PublishDateTime,
                    LatestUpdateDateTime = post.LatestUpdateDateTime,
                    Summary = post.Summary,
                    ThumbnailUrl = post.ThumbnailUrl,
                    PostUrl = GetPostUrl(post),
                    PostEditUrl = GetPostEditUrl(post),
                    postDeleteUrl = GetPostDeleteUrl(post),
                    postEditTypeUrl = GetPostEditTypeUrl(post),
                    RelatedPosts = MostRelatedPostsToTagsSummary(post),
                    Pin = post.Pin,
                    Posterpins = post.Posterpins
                });
            }
            return posts;
        }

        private static List<int> RetrievePostIdsForRelatedPosts(List<Post> posts)
        {
            var postIds = new List<int>();
            foreach (var post in posts)
                postIds.AddRange(RetrievePostIdsForRelatedPosts(post));

            return postIds;
        }

        private static List<int> RetrievePostIdsForRelatedPosts(Post post)
        {
            var postIds = new List<int>();
            foreach (var postTag in post.PostTags)
                foreach (var innerPostTag in postTag.Tag.PostTags)
                    if (!postIds.Any(pid => pid == innerPostTag.PostId)) postIds.Add(innerPostTag.PostId);

            return postIds;
        }

        private static List<Post> MostRelatedPostsToTags(Post post, int count = 10, int skip = 0)
        {
            var postsAndPoints = new List<RelatedPostPointViewModel>();
            foreach (var postTag in post.PostTags)
            {
                foreach (var innerPostTag in postTag.Tag.PostTags)
                {
                    if (innerPostTag.Post.Id == post.Id) continue;
                    else if (!postsAndPoints.Any(pp => pp.Post == innerPostTag.Post))
                        postsAndPoints.Add(new RelatedPostPointViewModel { Post = innerPostTag.Post, Point = 1 });
                    else
                        postsAndPoints.Where(pp => pp.Post == innerPostTag.Post).ToList().ForEach(pp => pp.Point++);
                }
            }


            postsAndPoints = postsAndPoints.OrderByDescending(pap => pap.Point).ThenByDescending(pap => Math.Abs(pap.Post.PublishDateTime.CompareTo(post.PublishDateTime))).Skip(skip).Take(count).ToList();

            var mostRelatedPosts = new List<Post>();
            postsAndPoints.ForEach(pap => mostRelatedPosts.Add(pap.Post));

            return mostRelatedPosts;
        }

        private static List<PostSummaryInShortViewModel> MostRelatedPostsToTagsSummary(Post post, int count = 2, int skip = 0)
        {
            var postsAndPoints = new List<RelatedPostPointViewModel>();
            foreach (var postTag in post.PostTags)
            {
                foreach (var innerPostTag in postTag.Tag.PostTags)
                {
                    if (innerPostTag.Post.Id == post.Id) continue;
                    else if (!postsAndPoints.Any(pp => pp.Post == innerPostTag.Post))
                        postsAndPoints.Add(new RelatedPostPointViewModel { Post = innerPostTag.Post, Point = 1 });
                    else
                        postsAndPoints.Where(pp => pp.Post == innerPostTag.Post).ToList().ForEach(pp => pp.Point++);
                }
            }


            postsAndPoints = postsAndPoints.OrderByDescending(pap => pap.Point).ThenByDescending(pap => Math.Abs(pap.Post.PublishDateTime.CompareTo(post.PublishDateTime))).Skip(skip).Take(count).ToList();

            var mostRelatedPosts = new List<PostSummaryInShortViewModel>();
            postsAndPoints.ForEach(pap => mostRelatedPosts.Add(
                new PostSummaryInShortViewModel
                {
                    Id = pap.Post.Id,
                    Title = pap.Post.Title,
                    LatestUpdateDateTime = pap.Post.LatestUpdateDateTime,
                    PostUrl = GetPostUrl(pap.Post),
                    PublishDateTime = pap.Post.PublishDateTime
                }));

            return mostRelatedPosts;
        }

        [Route("{postId}/related-posts.json")]
        public async Task<List<PostSummaryInShortViewModel>> MostRelatedPosts(int postId, int count = 5, int skip = 0)
        {
            var retrievePost = await _context.Posts
                .Include(p => p.Article)
                .Include(p => p.Share)
                .Include(p => p.Markdown)
                .Include(p => p.DestructivePosts)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag).ThenInclude(t => t.PostTags)
                .FirstOrDefaultAsync(post => post.Id == postId);
            if (retrievePost == null)
                return null;

            await _context.Posts.Where(p => RetrievePostIdsForRelatedPosts(retrievePost).Contains(p.Id))
                .Include(p => p.Article)
                .Include(p => p.Share)
                .Include(p => p.Markdown)
                .Include(p => p.DestructivePosts)
                .ToListAsync();

            return MostRelatedPostsToTagsSummary(post: retrievePost, count: count, skip: skip);
        }

        [Route("{postId}/related-posts")]
        public async Task<IActionResult> RelatedPosts(int postId, int count = 10, int skip = 0)
        {
            var retrievePost = await _context.Posts
                .Include(p => p.Article)
                .Include(p => p.Share)
                .Include(p => p.Markdown)
                .Include(p => p.DestructivePosts)
                .Include(p => p.PostTags).ThenInclude(pt => pt.Tag).ThenInclude(t => t.PostTags)
                .FirstOrDefaultAsync(post => post.Id == postId);
            if (retrievePost == null)
                return null;

            await _context.Posts.Where(p => RetrievePostIdsForRelatedPosts(retrievePost).Contains(p.Id))
                .Include(p => p.Article)
                .Include(p => p.Share)
                .Include(p => p.Markdown)
                .Include(p => p.DestructivePosts)
                .ToListAsync();

            return View(new RelatedPostsViewModel {
                TargetPost = new PostSummaryViewModel
                {
                    Id = retrievePost.Id,
                    Title = retrievePost.Title,
                    Type = DetectPostType(retrievePost),
                    PublishDateTime = retrievePost.PublishDateTime,
                    LatestUpdateDateTime = retrievePost.LatestUpdateDateTime,
                    Summary = retrievePost.Summary,
                    ThumbnailUrl = retrievePost.ThumbnailUrl,
                    PostUrl = GetPostUrl(retrievePost),
                    PostEditUrl = GetPostEditUrl(retrievePost),
                    postDeleteUrl = GetPostDeleteUrl(retrievePost),
                    postEditTypeUrl = GetPostEditTypeUrl(retrievePost),
                    RelatedPosts = MostRelatedPostsToTagsSummary(retrievePost)
                },
                RelatedPosts = MostRelatedPostsToTags(post: retrievePost, count: count, skip: skip)
                .Select(post => new PostSummaryViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Type = DetectPostType(post),
                    PublishDateTime = post.PublishDateTime,
                    LatestUpdateDateTime = post.LatestUpdateDateTime,
                    Summary = post.Summary,
                    ThumbnailUrl = post.ThumbnailUrl,
                    PostUrl = GetPostUrl(post),
                    PostEditUrl = GetPostEditUrl(post),
                    postDeleteUrl = GetPostDeleteUrl(post),
                    postEditTypeUrl = GetPostEditTypeUrl(post),
                    RelatedPosts = MostRelatedPostsToTagsSummary(post)
                }).ToList()
            });
        }

        //Detect PostType
        private async Task<PostType> DetectPostType(int postId)
                => DetectPostType(await _context.Posts.Where(p => p.Id == postId).Select(post => new Post { Article = post.Article, Share = post.Share, Markdown = post.Markdown }).FirstOrDefaultAsync());

        public static PostType DetectPostType(Post post)
        {
            if (post == null)
                throw new Exception("مطلبی یافت نشد!");

            bool isArticle = IsArticle(post),
                isShare = IsShare(post),
                isMarkdown = IsMarkdown(post);

            var postTypes = (new bool[] { isArticle, isShare, isMarkdown }).Where(postType => postType == true).Count();
            if (postTypes == 0)
                throw new Exception("در فرایند تشخیص نوع مطلب، نوعی پیدا نشد!");
            else if (postTypes > 1)
                throw new Exception("در فرایند تشخیص نوع مطلب، بیش از یک نوع پیدا شد!");

            if (isArticle) return PostType.Article;
            else if (isShare) return PostType.Share;
            else /*if (isMarkdown)*/ return PostType.Markdown;
        }

        private static bool IsArticle(Post post) => post.Article != null;

        private static bool IsMarkdown(Post post) => post.Markdown != null;

        private static bool IsShare(Post post) => post.Share != null;

        private static bool IsDestructive(Post post) => post.DestructivePosts != null;

        //Get post Address(es)
        public static string GetPostUrl(Post post)
        {
            return (DetectPostType(post)) switch
            {
                PostType.Article => $"/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}/{post.Article.UrlTitle}",
                PostType.Markdown => $"/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}/{post.Markdown.UrlTitle}",
                PostType.Share => post.Share.RedirectToUrl,
                _ => throw new Exception("در فرایند دریافت نشانی، نوع مطلب پیدا نشد."),
            };
        }

        private static string GetPostEditUrl(Post post)
        {
            return (DetectPostType(post)) switch
            {
                PostType.Article => $"/edit/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}/{post.Article.UrlTitle}",
                PostType.Markdown => $"/edit/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}/{post.Markdown.UrlTitle}",
                PostType.Share => $"/edit/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}",
                _ => throw new Exception("در فرایند دریافت مسیر ویرایش، نوع مطلب پیدا نشد."),
            };
        }

        private static string GetPostDeleteUrl(Post post)
        {
            return (DetectPostType(post)) switch
            {
                PostType.Article => $"/delete/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}/{post.Article.UrlTitle}",
                PostType.Markdown => $"/delete/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}/{post.Markdown.UrlTitle}",
                PostType.Share => $"/delete/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}",
                _ => throw new Exception("در فرایند دریافت مسیر حذف، نوع مطلب پیدا نشد."),
            };
        }

        private static string GetPostEditTypeUrl(Post post)
        {
            return (DetectPostType(post)) switch
            {
                PostType.Article => $"/edit/type/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}/{post.Article.UrlTitle}",
                PostType.Markdown => $"/edit/type/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}/{post.Markdown.UrlTitle}",
                PostType.Share => $"/edit/type/{post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd")}/{post.Id}",
                _ => throw new Exception("در فرایند دریافت مسیر تغییر نوع، نوع فعلی مطلب پیدا نشد."),
            };
        }
    }
}