using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ScooterRental.Core.Models;

namespace ScooterRental.Data.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Scooter> Scooters { get; set; }
        DbSet<RentedScooter> RentedScooters { get; set; }
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
