using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Models
{
    public class LoginModel
    {
        [Display(Name = "用户名", Prompt = "用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        public string Username { get; set; }

        [Display(Name = "密码", Prompt = "密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
    }
}
