using Microsoft.EntityFrameworkCore;
using Shopping.DAL.Configurations;
using Shopping.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.DAL
{
    public class ShoppingContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
