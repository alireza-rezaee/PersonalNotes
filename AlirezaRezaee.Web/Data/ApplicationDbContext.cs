using System;
using System.Collections.Generic;
using System.Text;
using AlirezaRezaee.Web.Areas.Identity.Data;
using AlirezaRezaee.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<Article> Articles { get; set; }

        //Categories for Articles
        public DbSet<ACategory> ACategories { get; set; }

        public DbSet<Share> Shares { get; set; }

        //Cateories for Share
        public DbSet<SCategory> SCategories { get; set; }
    }
}
