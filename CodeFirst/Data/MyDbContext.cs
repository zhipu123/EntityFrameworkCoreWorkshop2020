using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data
{
    public class MyDbContext : DbContext
    
    {
        public MyDbContext()
        {
            
        }
        public MyDbContext(DbContextOptions options): base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=codeFirst.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Post.ApplyPost(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Post> Posts { get; set; }
    }
}