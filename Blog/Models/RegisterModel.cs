using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Models
{
    public class RegisterModel
    {
        [Display(Name = "用户名", Prompt = "用户名")]
        [RegularExpression("^[A-Za-z0-9]{3,20}$", ErrorMessage = "用户名不合法，用户名只能包含字母及数字，长度为3到20位")]
        [Required(ErrorMessage = "用户名不能为空")]
        public string Username { get; set; }

        [Display(Name = "密码", Prompt = "密码")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "密码不能短于8位")]
        [MaxLength(20, ErrorMessage = "密码不能超过20位")]
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        [Display(Name = "重复密码", Prompt = "重复密码")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "密码不能短于8位")]
        [MaxLength(20, ErrorMessage = "密码不能超过20位")]
        [Compare("Password", ErrorMessage = "密码与重复密码不匹配")]
        [Required(ErrorMessage = "重复密码不能为空")]
        public string RepeatPassword { get; set; }

        [Display(Name = "Email", Prompt = "Email")]
        [EmailAddress(ErrorMessage = "电子邮箱格式错误")]
        [Required(ErrorMessage = "电子邮箱不能为空")]
        public string Email { get; set; }
    }
}
