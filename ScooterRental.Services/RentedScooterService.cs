using ScooterRental.Core.Models;
using ScooterRental.Core.Services;
using ScooterRental.Data.Data;
using ScooterRental.Services.Calculation;
using ScooterRental.Services.Validations;

namespace ScooterRental.Services
{
    public class RentedScooterService : EntityService<RentedScooter>, IRentedScooterService
    {
        public RentedScooterService(IApplicationDbContext context) : base(context){ }
        public ServiceResult StartRent(string id)
        {
            var time = DateTime.UtcNow;
            var scooter = _context.Scooters.FirstOrDefault(x => x.Id == id);
            var result = RentedScooterValidator.ScooterIsNullValidation(id, scooter);

            if (result != null && result.Errors.Count > 0)
            {
                return result;
            }

            var res = RentedScooterValidator.ScooterIsAlreadyRentedValidation(scooter);

            if (res != null && res.Errors.Count > 0)
            {
                return res;
            }

            scooter.IsRented = true;
            Update(scooter);

            return Create(new RentedScooter
            {
                Scooter = scooter,
                StartTime = time,
                PricePerMinute = scooter.PricePerMinute
            });
        }
        public ServiceResult EndRent(string id)
        {
            var time = DateTime.UtcNow;
            var scooter = _context.Scooters.FirstOrDefault(x => x.Id == id);
            var result = RentedScooterValidator.ScooterIsNullValidation(id, scooter);

            if (result != null && result.Errors.Count > 0)
            {
                return result;
            }
            var res = RentedScooterValidator.ScooterIsNotRentedValidation(scooter);

            if (res != null && res.Errors.Count > 0)
            {
                return res;
            }
            var rentedScooter = _context.RentedScooters.FirstOrDefault(x => x.Scooter == scooter && x.EndTime == null);

            rentedScooter.EndTime = time;
            rentedScooter.Scooter.IsRented = false;

            return Update(rentedScooter);  
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            return RentalCalculationService.TotalIncomeCalculation(year, includeNotCompletedRentals, GetAll().ToList());
        } 
    }
}
