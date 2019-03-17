using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Lsj.Util.Text;
using Lsj.Util.AspNetCore.PagedList;

namespace Blog.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(DataContext dataContext) : base(dataContext)
        {
        }

        public IActionResult Index(HomeModel model)
        {
            var articles = dataContext.Articles.AsQueryable();

            if (!model.Keyword.IsNullOrEmpty())
            {
                articles = articles.Where(x => x.ArticleTitle.Contains(model.Keyword));
            }
            else if (model.CategoryID != 0)
            {
                articles = articles.Where(x => x.ArticleCategoryID == model.CategoryID);
            }

            model.Articles = articles.ToPagedList(model.PageNum, 10);

            ViewBag.Categories = dataContext.Categories;


            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userid");
            return Redirect("/");
        }
    }
}
