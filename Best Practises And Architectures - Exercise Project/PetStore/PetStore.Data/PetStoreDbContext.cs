using System;
using Microsoft.EntityFrameworkCore;
using PetStore.Data.Models;

namespace PetStore.Data
{
    public class PetStoreDbContext : DbContext
    {
        public PetStoreDbContext()
        {
        }

        public PetStoreDbContext(DbContextOptions options)
        : base(options)
        {
        }

        public DbSet<Merchandise> Merchandises { get; set; }

        public DbSet<MerchandiseType> MerchandiseTypes{ get; set; }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Breed> Breeds { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer($@"Server=LAPTOP-FB9RKQ9D\SQLEXPRESS;Database=PetStore;Integrated Security=True");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetStoreDbContext).Assembly);
        }
    }
}