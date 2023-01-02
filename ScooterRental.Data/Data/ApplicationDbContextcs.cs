using Microsoft.EntityFrameworkCore;
using ScooterRental.Core.Models;

namespace ScooterRental.Data.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Scooter> Scooters { get; set; }
        public DbSet<RentedScooter> RentedScooters { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options){ }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RentedScooter>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        }
    }
}
