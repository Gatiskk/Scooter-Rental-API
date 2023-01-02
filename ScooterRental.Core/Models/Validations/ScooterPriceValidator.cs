using ScooterRental.Core.Interfaces;

namespace ScooterRental.Core.Models.Validations
{
    public class ScooterPriceValidator : IScooterValidator
    {
        public bool IsValid(Scooter scooter)
        {
            return scooter.PricePerMinute > 0;
        }
    }
}
