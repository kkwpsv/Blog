
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Http;
using Lsj.Util.Encrypt;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lsj.Util.AspNetCore.PagedList;
using Lsj.Util.AspNetCore.MVC;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        public AdminController(DataContext dataContext, IWebHostEnvironment environment) : base(dataContext)
        {
            this.hostingEnvironment = environment;
        }

        protected override bool CheckUserRight() => user != null && user.UserRole == UserRole.Admin;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile image)
        {
            if (image != null && image.Length != 0)
            {
                var buffer = new byte[image.Length];
                using (var stream = new MemoryStream(buffer))
                {
                    image.CopyTo(stream);
                }
                var filename = MD5.GetMD5String(buffer) + Path.GetExtension(image.FileName);
                System.IO.File.WriteAllBytes(Path.Combine(hostingEnvironment.WebRootPath, "image", filename), buffer);
                return new JsonResult(new { ImgUrl = "/image/" + filename });
            }
            return NotFound();
        }

        public IActionResult ArticleList(int pageNum = 1)
        {
            pageNum = pageNum > 1 ? pageNum : 1;
            return View(dataContext.Articles.OrderByDescending(x => x.ArticleID).ToPagedList(pageNum, 10));
        }

        [HttpGet]
        public IActionResult CreateArticle()
        {
            ViewBag.Categorys = dataContext.Categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryID.ToString() });
            return View("Article");
        }

        [HttpGet]
        public IActionResult EditArticle(int id)
        {
            var article = dataContext.Articles.SingleOrDefault(x => x.ArticleID == id);
            if (article != null)
            {
                ViewBag.Categorys = dataContext.Categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryID.ToString() });
                return View("Article", new ArticleModel
                {
                    ArticleID = id,
                    ArticleCategory = article.ArticleCategoryID,
                    ArticleContent = article.ArticleContent,
                    ArticleTitle = article.ArticleTitle,
                });
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult EditArticle(int id, ArticleModel articleModel)
        {
            Article article;
            if (id == 0)
            {
                article = new Article();
                dataContext.Add(article);
            }
            else
            {
                article = dataContext.Articles.SingleOrDefault(x => x.ArticleID == id);
            }
            if (article != null)
            {
                article.ArticleCategoryID = articleModel.ArticleCategory;
                article.ArticleContent = articleModel.ArticleContent;
                article.ArticleTitle = articleModel.ArticleTitle;
                article.ArticleSummary = Util.GetContentSummary(article.ArticleContent, 20, true);
                dataContext.SaveChanges();

                articleModel.ArticleID = article.ArticleID;

                ViewBag.Categorys = dataContext.Categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryID.ToString() });
                return View("Article", articleModel);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult DeleteArticle(int id)
        {
            var article = dataContext.Articles.SingleOrDefault(x => x.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }
            article.IsDeleted = true;
            dataContext.SaveChanges();
            return new JavaScriptResult { Content = "<script>alert('删除成功。');window.location.href='/Admin/ArticleList'</script>" };
        }

        [HttpGet]
        public IActionResult CategoryList(int pageNum = 1)
        {
            pageNum = pageNum > 1 ? pageNum : 1;
            return View(dataContext.Categories.Where(x => x.CategoryID != -1).ToPagedList(pageNum, 10));
        }

        [HttpPost]
        public IActionResult CategoryList(string categoryName, int pageNum = 1, int categoryID = 0)
        {
            if (categoryID == 0)
            {
                var category = new Category
                {
                    CategoryName = categoryName
                };
                dataContext.Add(category);
                dataContext.SaveChanges();
            }
            else
            {
                var category = dataContext.Categories.SingleOrDefault(x => x.CategoryID == categoryID);
                if (category != null)
                {
                    category.CategoryName = categoryName;
                    dataContext.SaveChanges();
                }
            }

            pageNum = pageNum > 1 ? pageNum : 1;
            return View(dataContext.Categories.Where(x => x.CategoryID != -1).ToPagedList(pageNum, 10));
        }

        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var category = dataContext.Categories.Include(x => x.Articles).SingleOrDefault(x => x.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }
            if (category.Articles.Count != 0)
            {
                return new JavaScriptResult { Content = "<script>alert('分类中含有文章，请先删除文章。');window.location.href='/Admin/CategoryList'</script>" };
            }
            category.IsDeleted = true;
            dataContext.SaveChanges();
            return RedirectToAction(nameof(CategoryList));
        }

        [HttpGet]
        public IActionResult UserList(int pageNum = 1)
        {
            pageNum = pageNum > 1 ? pageNum : 1;
            return View(dataContext.Users.ToPagedList(pageNum, 10));
        }

        [HttpPost]
        public IActionResult UpdatePassword(string password)
        {
            var user = dataContext.Users.SingleOrDefault(x => x.UserRole == UserRole.Admin);
            if (user != null)
            {
                user.Password = MD5.GetMD5String(password);
                dataContext.SaveChanges();
                return new JavaScriptResult { Content = "<script>alert('修改成功。');window.location.href='/Admin/UserList'</script>" };
            }
            return StatusCode(500);
        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            var user = dataContext.Users.SingleOrDefault(x => x.UserID == id && x.UserRole == UserRole.Guest);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDeleted = true;
            dataContext.SaveChanges();
            return new JavaScriptResult { Content = "<script>alert('修改成功。');window.location.href='/Admin/UserList'</script>" };
        }
    }
}
