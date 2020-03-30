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

        public DbSet<OptionModel> Options { get; set; }

        public DbSet<UserAboutModel> Abouts { get; set; }

        public DbSet<LinksModel> LinksModel { get; set; }
    }
}
