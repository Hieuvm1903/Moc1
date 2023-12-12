using Microsoft.EntityFrameworkCore;
using Moc.Entities;
using Moc.Models;

namespace Moc.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa { Id = 1, Name = "Beach view", Amenity = "full", CreatedDate = DateTime.Now, Sqft = 100, Rate = 4, ImageUrl = "https", Details = "HaLong Beach", Occupancy = 500 },
                new Villa { Id = 2, Name = "Mountain view", Amenity = "half", CreatedDate = DateTime.Now, Sqft = 200, Rate = 3.5, ImageUrl = "https", Details = "ThienVan Mount", Occupancy = 1000 },
                new Villa { Id = 3, Name = "ABC resort", Amenity = "full", CreatedDate = DateTime.Now, Sqft = 150, Rate = 5, ImageUrl = "https", Details = "DoSon Beach", Occupancy = 400 },
                new Villa { Id = 4, Name = "Lake view", Amenity = "temp", CreatedDate = DateTime.Now, Sqft = 75, Rate = 4, ImageUrl = "https", Details = "DongDo Lake", Occupancy = 100 }
                );
        }
    }
}
