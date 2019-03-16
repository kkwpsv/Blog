using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;

namespace Blog.Controllers
{
    public class ArticleController : BaseController
    {
        public ArticleController(DataContext dataContext) : base(dataContext)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
