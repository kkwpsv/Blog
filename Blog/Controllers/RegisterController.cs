using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Lsj.Util.Encrypt;
using Lsj.Util.AspNetCore.MVC;

namespace Blog.Controllers
{
    public class RegisterController : BaseController
    {
        public RegisterController(DataContext dataContext) : base(dataContext)
        {
        }

        public IActionResult Index()
        {
            HttpContext.Session.Remove("userid");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var username = registerModel.Username;
                var password = registerModel.Password;
                if (dataContext.Users.Any(x => x.Username == registerModel.Username))
                {
                    ModelState.AddModelError("Username", "用户名重复");
                    return View(registerModel);
                }

                var newUser = new User
                {
                    Email = registerModel.Email,
                    Password = MD5.GetMD5String(registerModel.Password),
                    Username = registerModel.Username,
                    UserRole = UserRole.Guest
                };
                this.dataContext.Add(newUser);
                this.dataContext.SaveChanges();
                return new JavaScriptResult { Content = "<script>alert('注册成功。');window.location.href='/'</script>" };
            }
            return View();
        }
    }
}
