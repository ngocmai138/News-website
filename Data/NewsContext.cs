using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using News.Models;

namespace News.Data
{
    public class NewsContext : DbContext
    {
        public NewsContext (DbContextOptions<NewsContext> options)
            : base(options)
        {
        }

        public DbSet<News.Models.Category> Category { get; set; } = default!;
        public DbSet<News.Models.User> User { get; set; } = default!;
        public DbSet<News.Models.Article> Article { get; set; } = default!;
        public DbSet<News.Models.Subscription> Subscription { get; set; } = default!;
    }
}
