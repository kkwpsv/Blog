using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Blog.Controllers
{
    public class BaseController : Controller
    {
        protected DataContext dataContext;
        protected User user;

        public BaseController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            if (userid != null)
            {
                this.user = this.dataContext.Users.Find(userid.Value);
            }

            if (CheckUserRight())
            {
                ViewBag.User = this.user;
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = NotFound();
            }
        }

        protected virtual bool CheckUserRight() => true;
    }
}
