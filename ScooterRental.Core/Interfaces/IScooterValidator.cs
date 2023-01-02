using ScooterRental.Core.Models;

namespace ScooterRental.Core.Interfaces
{
    public interface IScooterValidator
    {
        bool IsValid(Scooter scooter);
    }
}
