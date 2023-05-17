using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Order.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EndWerk.Project.Data
{
    public class Repository : IdentityDbContext<User>
    {
        //public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order.Object.Order> Orders { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public Repository()
        {
            
        }

        public Repository(DbContextOptions<Repository> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=OrderDB;User Id=orderdbmanger;Password=test123;TrustServerCertificate=True");
            }
        }
    }
}