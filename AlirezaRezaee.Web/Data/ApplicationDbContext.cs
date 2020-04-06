using System;
using System.Collections.Generic;
using System.Text;
using AlirezaRezaee.Web.Areas.Identity.Data;
using AlirezaRezaee.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlirezaRezaee.Web.Models.ViewModels.Posts;

namespace AlirezaRezaee.Web.Data
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
        }
    }
}
