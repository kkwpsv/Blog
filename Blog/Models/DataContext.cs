using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
    }

    public enum UserRole
    {
        Admin,
        Guest
    }

    public class User
    {
        public int UserID { get; set; }
        public UserRole UserRole { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PassWord { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public Category FatherCategory { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Article
    {
        public int ArticleID { get; set; }
        public Category ArticleCategory { get; set; }
        [Required]
        public string ArticleTitle { get; set; }
        [Required]
        public string ArticleContent { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Comment
    {
        public int CommentID { get; set; }
        [Required]
        public User CommentUser { get; set; }
        [Required]
        public Article CommentArticle { get; set; }
        [Required]
        public string CommentContent { get; set; }
        public bool IsDeleted { get; set; }
    }


}
