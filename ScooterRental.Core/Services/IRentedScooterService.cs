namespace ScooterRental.Core.Services
{
    public interface IRentedScooterService
    {
        ServiceResult StartRent(string id);
        ServiceResult EndRent(string id);
        decimal CalculateIncome(int? year, bool includeNotCompletedRentals);
    }
}
