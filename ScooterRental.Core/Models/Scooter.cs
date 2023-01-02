namespace ScooterRental.Core.Models
{
    public class Scooter : Entity
    {
        public bool IsRented { get; set; }
        public decimal PricePerMinute { get; set; }       
    }
}
