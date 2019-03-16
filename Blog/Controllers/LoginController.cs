using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Lsj.Util.Encrypt;
using Microsoft.AspNetCore.Http;
using Lsj.Util.AspNetCore.MVC;

namespace Blog.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(DataContext dataContext) : base(dataContext)
        {
        }

        public IActionResult Index()
        {
            HttpContext.Session.Remove("userid");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var username = loginModel.Username;
                var password = loginModel.Password;

                var user = dataContext.Users.Where(x => x.Username == loginModel.Username && x.Password == MD5.GetMD5String(loginModel.Password)).SingleOrDefault();
                if (user != null)
                {
                    this.HttpContext.Session.SetInt32("userid", user.UserID);
                    return new JavaScriptResult { Content = "<script>alert('登录成功。');window.location.href='/'</script>" };
                }
                else
                {
                    ModelState.AddModelError("", "用户名或密码错误。");
                }

                return View(loginModel);
            }
            return View();
        }
    }
}
