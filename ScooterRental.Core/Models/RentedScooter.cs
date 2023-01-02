namespace ScooterRental.Core.Models
{
    public class RentedScooter : Entity
    {
        public Scooter Scooter { get; set; }
        public DateTime StartTime { get; set; }
        public decimal PricePerMinute { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
