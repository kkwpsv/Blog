using Lsj.Util.AspNetCore.PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class HomeModel
    {
        public string Keyword { get; set; }

        public int CategoryID { get; set; }

        public IPagedList<Article> Articles { get; set; }

        public int PageNum { get; set; } = 1;
    }
}
