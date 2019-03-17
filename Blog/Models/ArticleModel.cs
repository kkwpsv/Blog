using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class ArticleModel
    {
        public int ArticleID { get; set; }

        [Display(Name = "分类", Prompt = "分类")]
        public int ArticleCategory { get; set; }

        [Display(Name = "标题", Prompt = "标题")]
        [Required(ErrorMessage = "标题不能为空")]
        public string ArticleTitle { get; set; }

        [Display(Name = "内容", Prompt = "内容")]
        [Required(ErrorMessage = "内容不能为空")]
        public string ArticleContent { get; set; }
    }
}
