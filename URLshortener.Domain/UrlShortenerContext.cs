using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLshortener.Domain.Entities;

namespace URLshortener.Domain
{
    public class UrlShortenerContext : DbContext
    {
        public UrlShortenerContext(DbContextOptions options) : base(options)
        {
        }

        protected UrlShortenerContext()
        {
        }

        public DbSet<User> Users { get; set; }
            
        public DbSet<UrlEntry> UrlEntries { get; set; }
        public DbSet<UserLink> UserLinks { get; set; }
        
    }
}
