using FluentAssertions;
using Moq;
using ScooterRental.Core.Models;
using ScooterRental.Core.Services;
using ScooterRental.Services;
using ScooterRental.Services.Calculation;
using ScooterRental.Tests.Utils;

namespace ScooterRental.Tests
{
    public class RentedScooterServiceTests : ScooterTestBase
    {
        private readonly RentedScooterService _rentedScooterService;
        private  DateTime _time = DateTime.UtcNow;

        public RentedScooterServiceTests()
        {
            _rentedScooterService = new RentedScooterService(DbContext);
            _time = _time.AddTicks(-(_time.Ticks % TimeSpan.TicksPerSecond));
        }

        [Fact]
        public async Task DatabaseCanBeConnectedTo()
        {
            Assert.True(await DbContext.Database.CanConnectAsync());
        }

        [Fact]
        public void StartRent_ScooterExists_ShouldStartRent()
        {
            //Arrange
            var scooter = new Scooter() { Id = "1", IsRented = false, PricePerMinute = 1 };
            DbContext.Scooters.Add(scooter);
            DbContext.SaveChanges();

            //Act
            var result = _rentedScooterService.StartRent("1");

            //Assert
            DbContext.Scooters.First().IsRented.Should().BeTrue();
            var period = DbContext.RentedScooters.First();
            period.Scooter.Should().BeEquivalentTo(scooter);
            period.EndTime.Should().Be(null);
            period.PricePerMinute.Should().Be(1);
            ((RentedScooter)result.Entity).Scooter.Should().BeEquivalentTo(scooter);
            ((RentedScooter)result.Entity).EndTime.Should().Be(null);
            ((RentedScooter)result.Entity).PricePerMinute.Should().Be(1);
        }

        [Fact]
        public void StartRent_ScooterDoesNotExist_ThrowScooterNotFoundError()
        {
            //Act
            var result = _rentedScooterService.StartRent("testid");

            //Assert
            result.FormattedErrors.Should().Be($"Scooter id testid not found");
        }

        [Fact]
        public void StartRent_ScooterAlreadyRented_ThrowScooterIsAlreadyRentedError()
        {
            //Arrange
            var scooter = new Scooter() { Id = "1", IsRented = true, PricePerMinute = 1 };
            DbContext.Scooters.Add(scooter);
            DbContext.SaveChanges();

            //Act
            var result = _rentedScooterService.StartRent("1");

            //Assert
            result.FormattedErrors.Should().Be($"Scooter id 1 is already rented");
        }

        [Fact]
        public void EndRent_ScooterDoesNotExist_ThrowScooterNotFoundError()
        {
            //Act
            var result = _rentedScooterService.EndRent("testId");

            //Assert
            result.FormattedErrors.Should().Be("Scooter id testId not found");
        }

        [Fact]
        public void EndRent_ScooterNotRented_ThrowScooterNotRentedError()
        {
            //Arrange
            var scooter = new Scooter() { Id = "5", IsRented = false, PricePerMinute = 1 };
            DbContext.Scooters.Add(scooter);
            DbContext.SaveChanges();

            //Act
            var result = _rentedScooterService.EndRent("5");

            //Assert
            result.FormattedErrors.Should().Be($"Scooter id 5 is not rented");
        }

        [Fact]
        public void CalculateIncome_AllCompletedRentals_ShouldReturnIncome()
        {
            //Arrange
            var date = new DateTime(2022, 3, 1);
            DbContext.RentedScooters.Add(new RentedScooter()
            {
                Id = "1",
                StartTime = date,
                PricePerMinute = 10
            });
            DbContext.SaveChanges();
            DbContext.RentedScooters.First().EndTime = date.AddMinutes(10);
            DbContext.SaveChanges();

            //Assert
            _rentedScooterService.CalculateIncome(null, false).Should().Be(20);
        }

        [Fact]
        public void CalculateIncome_AllRentals_ShouldReturnIncome()
        {
            //Arrange
            var date = _time - TimeSpan.FromMinutes(10);
            DbContext.RentedScooters.Add(new RentedScooter()
            {
                Id = "1",
                StartTime = date,
                PricePerMinute = 1
            });
            DbContext.SaveChanges();

            //Assert
            _rentedScooterService.CalculateIncome(null, true).Should().Be(10);
        }
    }
}
