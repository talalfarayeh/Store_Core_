using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Store.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierID)
             .OnDelete(DeleteBehavior.Restrict);

          
            modelBuilder.Entity<Product>()
             .HasMany(p => p.Orders)
             .WithOne(o => o.Product)
             .HasForeignKey(o => o.ProductID)
             .OnDelete(DeleteBehavior.Cascade);

          
            modelBuilder.Entity<Product>()
                .HasMany(p => p.StockMovements)
                .WithOne(sm => sm.Product)
                .HasForeignKey(sm => sm.ProductID);


            modelBuilder.Entity<Supplier>()
                .Property(s => s.SupplierName)
                    .HasMaxLength(100)
                .IsRequired();


            modelBuilder.Entity<Product>()
                .Property(p => p.ProductName)
                .HasMaxLength(100)
                .IsRequired();

          
            modelBuilder.Entity<Order>()
                .Property(o => o.CustomerName)
                .HasMaxLength(100)
                .IsRequired();


        }
    }
}
