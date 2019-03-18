using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    public class ArticleController : BaseController
    {
        public ArticleController(DataContext dataContext) : base(dataContext)
        {
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var article = dataContext.Articles.Where(x => x.ArticleID == id).Include(x => x.Comments).ThenInclude(x => x.CommentUser).SingleOrDefault();
            if (article != null)
            {
                ViewBag.Categories = dataContext.Categories;
                return View(article);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Index(int id, string content)
        {
            if (this.user == null)
            {
                return NotFound();
            }
            var article = dataContext.Articles.Where(x => x.ArticleID == id).Include(x => x.Comments).ThenInclude(x => x.CommentUser).SingleOrDefault();
            if (article != null)
            {
                ViewBag.Categories = dataContext.Categories;
                var comment = new Comment
                {
                    CommentArticle = article,
                    CommentContent = content,
                    CommentUser = this.user
                };
                dataContext.Add(comment);
                dataContext.SaveChanges();
                return View(article);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
