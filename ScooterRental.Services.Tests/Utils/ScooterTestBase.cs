using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ScooterRental.Data.Data;

namespace ScooterRental.Tests.Utils
{
    public class ScooterTestBase
    {
        private const string InMemoryConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection;

        protected readonly ApplicationDbContext DbContext;

        protected ScooterTestBase()
        {
            _connection = new SqliteConnection(InMemoryConnectionString);
            _connection.CreateFunction("NEWID", () => Guid.NewGuid());
            _connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;
            DbContext = new ApplicationDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}
