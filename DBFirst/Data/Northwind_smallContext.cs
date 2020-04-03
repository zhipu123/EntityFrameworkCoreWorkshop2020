using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DBFirst.Models;

namespace DBFirst.Data
{
    public partial class Northwind_smallContext : DbContext
    {
        public Northwind_smallContext()
        {
        }

        public Northwind_smallContext(DbContextOptions<Northwind_smallContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
        public virtual DbSet<CustomerDemographic> CustomerDemographic { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeTerritory> EmployeeTerritory { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductDetailsV> ProductDetailsV { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Shipper> Shipper { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<Territory> Territory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http: //go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlite("Data Source=Northwind_small.sqlite");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<Employee>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity
                    .HasOne<Customer>(e => e.Customer)
                    .WithMany()
                    .HasForeignKey(order => order.CustomerId)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Product>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<ProductDetailsV>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ProductDetails_V");
            });

            modelBuilder.Entity<Region>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<Shipper>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            modelBuilder.Entity<Supplier>(entity => { entity.Property(e => e.Id).ValueGeneratedNever(); });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}