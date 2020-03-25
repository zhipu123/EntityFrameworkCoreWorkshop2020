using CodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data
{
    public class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=codeFirst.db");
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Author> Authors { get; set; }
    }
}