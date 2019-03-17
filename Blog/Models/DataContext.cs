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

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasIndex(x => x.CategoryName).IsUnique();
            modelBuilder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Article>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Comment>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Category>().HasData(new Category { CategoryID = -1, CategoryName = "未分类" });
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
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Category
    {
        public int CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Article
    {
        public int ArticleID { get; set; }
        [ForeignKey("ArticleCategoryID")]
        public Category ArticleCategory { get; set; }
        public int ArticleCategoryID { get; set; }
        [Required]
        public string ArticleTitle { get; set; }
        [Required]
        public string ArticleContent { get; set; }
        [Required]
        public string ArticleSummary { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Comment> Comments { get; set; }
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
