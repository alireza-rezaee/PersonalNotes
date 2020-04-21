using System;
using System.Collections.Generic;
using System.Text;
using Rezaee.Alireza.Web.Areas.Identity.Data;
using Rezaee.Alireza.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Models.ViewModels.Posts;

namespace Rezaee.Alireza.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Option> Options { get; set; }

        public DbSet<UserAbout> Abouts { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Share> Shares { get; set; }

        public DbSet<Markdown> Markdowns { get; set; }

        public DbSet<Recommendeds> Recommendeds { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Article>(entity => {
                entity.HasKey(a => a.PostId);
                entity.HasOne(a => a.Post)
                    .WithOne(a => a.Article)
                    .HasForeignKey<Article>(a => a.PostId);
            });

            builder.Entity<Share>(entity => {
                entity.HasKey(s => s.PostId);
                entity.HasOne(s => s.Post)
                    .WithOne(s => s.Share)
                    .HasForeignKey<Share>(s => s.PostId);
            });

            builder.Entity<Markdown>(entity => {
                entity.HasKey(s => s.PostId);
                entity.HasOne(s => s.Post)
                    .WithOne(s => s.Markdown)
                    .HasForeignKey<Markdown>(s => s.PostId);
            });

            builder.Entity<Recommendeds>(entity => {
                entity.HasKey(s => s.PostId);
                entity.HasOne(s => s.Post)
                    .WithOne(s => s.RecommendedPost)
                    .HasForeignKey<Recommendeds>(s => s.PostId);
            });
        }

        public DbSet<Rezaee.Alireza.Web.Models.ViewModels.Posts.PostSummaryViewModel> PostSummaryViewModel { get; set; }
    }
}
