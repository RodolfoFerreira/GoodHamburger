using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GoodHamburger.Infrastructure.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "GoodHamburgerDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = (int)EnumCategory.Sandwich, Name = "Sandwich" },
                new Category { Id = (int)EnumCategory.Beverage, Name = "Beverage" },
                new Category { Id = (int)EnumCategory.Garnish, Name = "Garnish" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Name = "X Burger", Price = 5.00, Category = null },
                new Product { Id = 2, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Name = "X Egg", Price = 4.50, Category = null },
                new Product { Id = 3, CategoryId = (int)EnumCategory.Sandwich, IsExtra = false, Name = "X Bacon", Price = 7.00, Category = null },
                new Product { Id = 4, CategoryId = (int)EnumCategory.Beverage, IsExtra = true, Name = "Soft Drink", Price = 2.50, Category = null, },
                new Product { Id = 5, CategoryId = (int)EnumCategory.Garnish, IsExtra = true, Name = "Fries", Price = 2.00, Category = null }
            );

            base.OnModelCreating(modelBuilder);

            _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderProduct> OrderProduct { get; set; }
    }
}
