using ScooterRental.Core.Models;

namespace ScooterRental.Services.Calculation
{
    public class RentalCalculationService
    {
        public static decimal TotalPrice(DateTime startDate, DateTime endDate, decimal pricePerMinute)
        {
            decimal totalPrice = 0;

            while (true)
            {
                int rentedMinutes = (int)endDate.Subtract(startDate).TotalMinutes;
                decimal price = 0;
                decimal tmpPrice = (rentedMinutes * pricePerMinute);
                bool isPast = DateTime.Compare(startDate.Date, endDate.Date) == -1;
                bool isPresent = DateTime.Compare(startDate.Date, endDate.Date) == 0;

                if (tmpPrice >= 20 && isPast)
                {
                    price += 20;
                    startDate = NextDay(startDate);
                }

                totalPrice += price;

                if (isPresent)
                {
                    rentedMinutes = (int)endDate.Subtract(startDate).TotalMinutes;
                    tmpPrice = (decimal)(rentedMinutes * (double)pricePerMinute);
                    price = 0;

                    if (tmpPrice > 20)
                    {
                        price += 20;
                    }
                    else
                    {
                        price += tmpPrice;
                    }

                    totalPrice += price;
                    break;
                }
            }

            return totalPrice;
        }

        private static DateTime NextDay(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            int days = dateTime.Day;
            int hours = dateTime.Hour;
            int minutes = dateTime.Minute;
            int seconds = dateTime.Second;

            return new DateTime(year, month, days, 0, 0, 0).AddDays(1);
        }

        public static decimal TotalIncomeCalculation(int? year, bool includeNotCompletedRentals, List<RentedScooter> rentedScooters)
        {
            List<RentedScooter> newRentedScooters;

            decimal totalSum = 0m;

            if (year.HasValue)
            {
                newRentedScooters = NewRentedScooters(year, includeNotCompletedRentals, rentedScooters);
            }
            else
            {
                newRentedScooters = new List<RentedScooter>(rentedScooters);
            }

            foreach (var rentedScooter in newRentedScooters)
            {
                totalSum = RentedScootersTotalSum(includeNotCompletedRentals, rentedScooter, totalSum);
            }

            return totalSum;
        }

        private static List<RentedScooter> NewRentedScooters(int? year, bool includeNotCompletedRentals, List<RentedScooter> rentedScooters)
        {
            List<RentedScooter> newRentedScooters;

            if (!includeNotCompletedRentals)
            {
                var query = RentedScootersEndTimeIsNotNullQuery(year, rentedScooters);

                newRentedScooters = query.ToList();
            }
            else
            {
                var query = RentedScootersEndTimeIsNullQuery(year, rentedScooters);

                newRentedScooters = query.ToList();
            }

            return newRentedScooters;
        }

        private static decimal RentedScootersTotalSum(bool includeNotCompletedRentals, RentedScooter rentedScooter, decimal totalSum)
        {
            if (rentedScooter.EndTime == null && includeNotCompletedRentals)
            {
                totalSum += TotalPrice(rentedScooter.StartTime, DateTime.UtcNow, rentedScooter.PricePerMinute);
            }
            else
            {
                totalSum = RentedScootersEndTimeNotNullTotalSum(rentedScooter, totalSum);
            }

            return totalSum;
        }

        private static decimal RentedScootersEndTimeNotNullTotalSum(RentedScooter rentedScooter, decimal totalSum)
        {
            if (rentedScooter.EndTime != null)
                totalSum += TotalPrice(rentedScooter.StartTime, rentedScooter.EndTime.Value, rentedScooter.PricePerMinute);
            return totalSum;
        }

        private static IEnumerable<RentedScooter> RentedScootersEndTimeIsNullQuery(int? year, List<RentedScooter> rentedScooters)
        {
            var query =
                from rentedScooter in rentedScooters
                where rentedScooter.EndTime == null || rentedScooter.EndTime.Value.Year == year.Value
                select rentedScooter;
            return query;
        }

        private static IEnumerable<RentedScooter> RentedScootersEndTimeIsNotNullQuery(int? year, List<RentedScooter> rentedScooters)
        {
            var query =
                from rentedScooter in rentedScooters
                where rentedScooter.EndTime != null && rentedScooter.EndTime.Value.Year == year.Value
                select rentedScooter;
            return query;
        }
    }
}
