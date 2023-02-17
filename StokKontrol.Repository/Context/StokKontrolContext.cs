using Microsoft.EntityFrameworkCore;
using StokKontrol.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol.Repository.Context
{
    public class StokKontrolContext : DbContext
    {
        public StokKontrolContext(DbContextOptions<StokKontrolContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("server=DESKTOP-491CL38\\YAYLALISERVER22;database=StokKontrolDb;Trusted_Connection=True;");
            //hangi server'a bağlanacaksan bağlantı yapısını cennectionstrings.com ddan öğrenebilirin

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
