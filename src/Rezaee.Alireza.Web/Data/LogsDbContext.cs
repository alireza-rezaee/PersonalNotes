using Microsoft.EntityFrameworkCore;
using Rezaee.Alireza.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Data
{
    public class LogsDbContext : DbContext
    {
        public LogsDbContext(DbContextOptions<LogsDbContext> options)
            : base(options)
        {

        }

        public DbSet<RequestResponse> RequestResponse { get; set; }
    }
}
