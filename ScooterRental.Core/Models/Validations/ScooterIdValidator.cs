using ScooterRental.Core.Interfaces;

namespace ScooterRental.Core.Models.Validations
{
    public class ScooterIdValidator : IScooterValidator
    {
        public bool IsValid(Scooter scooter)
        {
            return !string.IsNullOrEmpty(scooter.Id);
        }
    }
}
