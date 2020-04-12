using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AlirezaRezaee.Web.Data;
using AlirezaRezaee.Web.Extensions;
using AlirezaRezaee.Web.Helpers;
using AlirezaRezaee.Web.Helpers.Enums;
using AlirezaRezaee.Web.Models;
using AlirezaRezaee.Web.Models.ViewModels.Posts;
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
            foreach (var post in await _context.Posts.Include(p => p.Article).Include(p => p.Share).OrderByDescending(p => p.PublishDateTime).Take(8).ToListAsync())
            {
                var postType = DetectPostType(post.Id);
                switch (postType.Result)
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
                            PostUrl = post.PublishDateTime.ToPersianDateTime().ToString("yyyy/MM/dd/") + $"{post.Id}/{post.Article.UrlTitle}"
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
                            PostUrl = post.Share.RedirectToUrl
                        });
                        break;
                    default:
                        continue; //نباید تحت هیچ عنوان به اینجا وارد بشه، باید تدابیری اندیشه بشه که اگر چنین اتفاقی افتاد مدیر سایت باخبر بشه، از طریق ایمیل یا لاگ انداختن
                }
            }
            return View(posts);
        }

        private async Task<PostType?> DetectPostType(int postId)
        {
            var post = await _context.Posts.Where(p => p.Id == postId).Select(p => new { p.Article, p.Share }).FirstOrDefaultAsync();

            if (post == null)
                return null;

            if ((post.Article == null && post.Share == null) || (post.Article != null && post.Share != null))
                return null;

            if (post.Article != null)
                return PostType.Article;
            else // if (post.Share != null)
                return PostType.Share;
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

            var postType = DetectPostType(postId).Result;

            if (postType == null) //Because this will check existance too
                return NotFound();

            if (postType == PostType.Article)
                return View(await _context.Posts.Include(p => p.Article).Where(p => dateTime.Date == p.PublishDateTime.Date && postId == p.Id).FirstOrDefaultAsync());
            else if (postType == PostType.Share)
                return Redirect(await _context.Posts.Include(p => p.Article).Where(p => dateTime.Date == p.PublishDateTime.Date && postId == p.Id).Select(p => p.Share.RedirectToUrl).FirstOrDefaultAsync());
            else
                return NotFound();
        }

        [HttpGet]
        [Route("delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> Delete(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var postType = DetectPostType(postId).Result;

            if (postType == null) //Because this will check existance too
                return NotFound();

            if (postType == PostType.Article)
                return View(await _context.Posts.Include(p => p.Article).Where(p => dateTime.Date == p.PublishDateTime.Date && postId == p.Id).FirstOrDefaultAsync());
            else if (postType == PostType.Share)
                return Redirect(await _context.Posts.Include(p => p.Share).Where(p => dateTime.Date == p.PublishDateTime.Date && postId == p.Id).Select(p => p.Share.RedirectToUrl).FirstOrDefaultAsync());
            else
                return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("delete/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> DeleteDone(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var postType = DetectPostType(postId).Result;

            if (postType == null) //Because this will check existance too
                return NotFound();

            if (postType == PostType.Article)
            {
                var post = await _context.Posts.Include(p => p.Article).Where(p => dateTime.Date == p.PublishDateTime.Date && postId == p.Id).FirstOrDefaultAsync();

                _ifileManager.DeleteFile(post.ThumbnailUrl);
                _ifileManager.DeleteFile(post.Article.CoverUrl);

                _context.Remove(post.Article);
                _context.Remove(post);
                await _context.SaveChangesAsync();
            }
            else if (postType == PostType.Share)
            {
                var post = await _context.Posts.Include(p => p.Article).Where(p => dateTime.Date == p.PublishDateTime.Date && postId == p.Id).FirstOrDefaultAsync();

                _ifileManager.DeleteFile(post.ThumbnailUrl);

                _context.Remove(post.Share);
                _context.Remove(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditArticlePostViewModel createPostVM)
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

            return View(createPostVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShare(CreateEditSharePostViewModel createShareVM)
        {
            if (ModelState.IsValid)
            {
                var persianDateTime = PersianDateTime.Now;
                var dateTime = persianDateTime.ToDateTime();

                try
                {
                    createShareVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });

                    var thumbnailPath = Path.Combine(
                        "uploads/images/",
                        persianDateTime.ToString("yyyy/MM/dd"),
                        persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + new Random().Next(1000000, 9999999) + "-" + createShareVM.ThumbnailImage.FileName);

                    _ifileManager.SaveFile(createShareVM.ThumbnailImage, thumbnailPath);

                    createShareVM.Post.ThumbnailUrl = thumbnailPath;

                    createShareVM.Post.PublishDateTime = dateTime;

                    createShareVM.Share.Post = createShareVM.Post;
                    _context.Add(createShareVM.Share);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    TempData["PostCreateStatus"] = e.Message;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(createShareVM);
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
                return RedirectToRoute("EditArticle", new { year = year, month = month, day = day, postId = postId, UrlTitle = "1" });
            }
            else if (postType == PostType.Share)
            {
                var post = await _context.Posts.Where(p => p.Id == postId).Include(p => p.Share).FirstOrDefaultAsync();
                return RedirectToRoute("EditArticle", new { year = year, month = month, day = day, postId = postId, UrlTitle = "1" });
            }
            else
                return NotFound();
        }

        [HttpGet]
        [Route("edit/article/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}", Name = "EditArticle")]
        public async Task<IActionResult> EditArticle(int year, int month, int day, int postId, string UrlTitle)
        {
            var dateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

            var post = await _context.Posts.Include(p => p.Article).Where(p => p.PublishDateTime.Year == dateTime.Year && p.PublishDateTime.Month == dateTime.Month && p.PublishDateTime.Day == dateTime.Day && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

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
            var previousPost = await _context.Posts.Where(p => p.Id == postId).Include(p => p.Article).Include(p => p.Share).AsNoTracking().FirstOrDefaultAsync();
            if (previousPost == null)
                return NotFound();

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
                        _ifileManager.DeleteFile(previousPost.Article.CoverUrl);
                        await _ifileManager.SaveFile(editPostVM.CoverImage, coverPath);
                        editPostVM.Article.CoverUrl = $"/{coverPath}";
                    }
                    else editPostVM.Article.CoverUrl = previousPost.Article.CoverUrl;

                    if (editPostVM.ThumbnailImage != null)
                    {
                        var thumbnailPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{ValidateName(editPostVM.ThumbnailImage.FileName)}";
                        _ifileManager.DeleteFile(previousPost.ThumbnailUrl);
                        await _ifileManager.SaveFile(editPostVM.ThumbnailImage, thumbnailPath);
                        editPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    }
                    else editPostVM.Post.ThumbnailUrl = previousPost.ThumbnailUrl;

                    if (previousPost.Share != null) _context.Remove(previousPost.Share);

                    editPostVM.Post.LatestUpdateDateTime = dateTime;
                    editPostVM.Article.PostId = postId;
                    editPostVM.Post.Id = postId;
                    editPostVM.Post.Article = editPostVM.Article;

                    _context.Update(editPostVM.Post);
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
        [Route("edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        public async Task<IActionResult> EditShare(CreateEditArticlePostViewModel editPostVM, int year, int month, int day, int postId, string UrlTitle)
        {
            if (!_context.Posts.Any(e => e.Id == postId))
                return NotFound();

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
                        editPostVM.CoverImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var coverPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{ValidateName(editPostVM.CoverImage.FileName)}";
                        await _ifileManager.SaveFile(editPostVM.CoverImage, coverPath);
                        editPostVM.Article.CoverUrl = $"/{coverPath}";
                    }

                    if (editPostVM.ThumbnailImage != null)
                    {
                        editPostVM.ThumbnailImage.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
                        var thumbnailPath = $"{imagePath}/{persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999)}_{ValidateName(editPostVM.ThumbnailImage.FileName)}";
                        await _ifileManager.SaveFile(editPostVM.ThumbnailImage, thumbnailPath);
                        editPostVM.Post.ThumbnailUrl = $"/{thumbnailPath}";
                    }

                    editPostVM.Post.PublishDateTime = dateTime;

                    editPostVM.Article.Post = editPostVM.Post;
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Route("edit/{year:int:range(1398,9378)}/{month:int:range(1,12)}/{day:int:range(1,31)}/{postId}/{UrlTitle?}")]
        //public async Task<IActionResult> Edit(int year, int month, int day, int postId, string UrlTitle, CreateEditArticlePostViewModel editArticleVM)
        //{
        //    var publishedDateTime = PersianDateTime.Parse($"{year:D4}/{month:D2}/{day:D2}").ToDateTime();

        //    var post = await _context.Posts.Where(p => p.Id == postId).Include(p => p.Article).Include(p => p.Share).FirstOrDefaultAsync();

        //    if (post == null)
        //        return NotFound();


        //    if (ModelState.IsValid)
        //    {
        //        var persianDateTime = PersianDateTime.Now;
        //        var dateTime = persianDateTime.ToDateTime();

        //        //From here
        //        if (editArticleVM.ThumbnailImage == null && editArticleVM.CoverImage == null)
        //        {
        //            try
        //            {
        //                if (editArticleVM.Article.UrlTitle == null)
        //                    editArticleVM.Article.UrlTitle = editArticleVM.Post.Title;
        //                foreach (char c in Path.GetInvalidFileNameChars())
        //                    editArticleVM.Article.UrlTitle = editArticleVM.Article.UrlTitle.Replace(c, '-');


        //                editArticleVM.Post.LatestUpdateDateTime = dateTime;
        //                editArticleVM.Article.Post = editArticleVM.Post;

        //                //if (post.Share != null)
        //                //    _context.Remove(post.Share);

        //                _context.Update(editArticleVM.Article);
        //                await _context.SaveChangesAsync();
        //                TempData["ArticleEditStatus"] = "OK";
        //            }
        //            catch (Exception e)
        //            {
        //                TempData["ArticleEditStatus"] = e.Message;
        //                return View(editArticleVM);
        //            }
        //        }
        //        else
        //        {
        //            try
        //            {
        //                var randomNumber = new Random();
        //        var imagePath = Path.Combine("uploads/images/", persianDateTime.ToString("yyyy/MM/dd"));


        //                if (editViewModel.Article.UrlTitle == null)
        //                    editViewModel.Article.UrlTitle = editViewModel.Article.Title;
        //                foreach (char c in Path.GetInvalidFileNameChars())
        //                    editViewModel.Article.UrlTitle = editViewModel.Article.UrlTitle.Replace(c, '-');


        //                if (editViewModel.Thumbnail != null && editViewModel.Cover != null)
        //                {
        //                    editViewModel.Thumbnail.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
        //                    var thumbnailPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Thumbnail.FileName);
        //    _ifileManager.SaveFile(editViewModel.Thumbnail, thumbnailPath);
        //                    editViewModel.Article.ThumbnailUrl = thumbnailPath;

        //                    editViewModel.Cover.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
        //                    var coverPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Cover.FileName);
        //_ifileManager.SaveFile(editViewModel.Cover, coverPath);
        //                    editViewModel.Article.CoverUrl = coverPath;
        //                }
        //                else if (editViewModel.Thumbnail != null)
        //                {
        //                    editViewModel.Thumbnail.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
        //                    var thumbnailPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Thumbnail.FileName);
        //_ifileManager.SaveFile(editViewModel.Thumbnail, thumbnailPath);
        //                    editViewModel.Article.ThumbnailUrl = thumbnailPath;
        //                }
        //                else // if (editViewModel.Cover != null)
        //                {
        //                    editViewModel.Cover.Check(1048576, new string[] { "image/jpg", "image/jpeg", "image/png", "image/gif" });
        //                    var coverPath = Path.Combine(imagePath, persianDateTime.ToString("yyyyMMddhhmmss") + DateTime.Now.ToString("ffff") + randomNumber.Next(1000000, 9999999) + "-" + editViewModel.Cover.FileName);
        //_ifileManager.SaveFile(editViewModel.Cover, coverPath);
        //                    editViewModel.Article.CoverUrl = coverPath;
        //                }

        //                editViewModel.Article.LatestUpdateDateTime = dateTime;

        //                _context.Update(editViewModel.Article);
        //                await _context.SaveChangesAsync();
        //TempData["ArticleEditStatus"] = "OK";
        //            }
        //            catch (Exception e)
        //            {
        //                TempData["ArticleEditStatus"] = e.Message;
        //                return View(editViewModel);
        //            }
        //        }

        //        return RedirectToAction("Details",
        //            new
        //            {
        //                year = dateTime.Year,
        //                month = dateTime.Month,
        //                day = dateTime.Day,
        //                postId = postId,
        //                UrlTitle = editArticleVM.Article.UrlTitle

        //            });
        //    }

        //    return View(editArticleVM);
        //}
    }
}