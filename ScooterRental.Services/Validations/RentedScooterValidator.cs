using ScooterRental.Core.Models;
using ScooterRental.Core.Services;

namespace ScooterRental.Services.Validations
{
    public class RentedScooterValidator
    {
        public static ServiceResult ScooterIsAlreadyRentedValidation(Scooter scooter)
        {
            if (scooter.IsRented)
            {
                return new ServiceResult(false).AddError($"Scooter id {scooter.Id} is already rented");
            }
            return null ;
        }

        public static ServiceResult ScooterIsNullValidation(string id, Scooter scooter)
        {
            if (scooter == null)
            {
                return new ServiceResult(false).AddError($"Scooter id {id} not found");
            }
            return null;
        }

        public static ServiceResult ScooterIsNotRentedValidation(Scooter scooter)
        {
            if (!scooter.IsRented)
            {
                return new ServiceResult(false).AddError($"Scooter id {scooter.Id} is not rented");
            }
            return null;
        }
    }
}
