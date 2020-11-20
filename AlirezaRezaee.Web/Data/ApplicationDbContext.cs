using System;
using System.Collections.Generic;
using System.Text;
using Rezaee.Alireza.Web.Areas.Identity.Data;
using Rezaee.Alireza.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Models.ViewModels.Posts;
using Rezaee.Alireza.Web.Areas.Admin.Models;

namespace Rezaee.Alireza.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Personalization> Personalizations { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Share> Shares { get; set; }

        public DbSet<Markdown> Markdowns { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<Pin> Pins { get; set; }

        public DbSet<Posterpins> Posterpins { get; set; }

        public DbSet<Block> Blocks { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<File> Files { get; set; }

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

            //Posts -> (many to many) <- Tags
            builder.Entity<PostTag>()
                .HasKey(pt => new { pt.PostId, pt.TagId });

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);


            builder.Entity<Pin>().HasIndex(pin => pin.PostId).IsUnique();

            builder.Entity<Posterpins>().HasIndex(pin => pin.PostId).IsUnique();
        }
    }
}
