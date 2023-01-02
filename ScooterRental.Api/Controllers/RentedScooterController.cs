using Microsoft.AspNetCore.Mvc;
using ScooterRental.Core.Services;

namespace ScooterRental_Api.Controllers
{
    [Route("rentedScooter-api")]
    [ApiController]
    public class RentedScooterController : Controller
    {
        private readonly IRentedScooterService _rentedScooterService;

        public RentedScooterController(IRentedScooterService rentalService)
        {
            _rentedScooterService = rentalService;
        }

        [HttpPost]
        [Route("startRent/scooter/{id}")]
        public IActionResult StartRent(string id)
        {
            var result = _rentedScooterService.StartRent(id);

            if (!result.Success) 
            {
                return BadRequest(result.FormattedErrors);
            } 
            return Ok(result.Entity);
        }

        [HttpPost]
        [Route("endRent/scooter/{id}")]
        public IActionResult EndRent(string id )
        {
            var result = _rentedScooterService.EndRent(id);

            if (!result.Success) 
            {
                return BadRequest(result.FormattedErrors);
            } 
            return Ok(result.Entity);
        }

        [HttpGet]
        [Route("totalIncome")]
        public IActionResult GetIncome(int? year, bool includeIncompleteRentals, DateTime? currentTime)
        {
            if (includeIncompleteRentals && currentTime == null)
            {
                return BadRequest();
            }
                
            if (currentTime != null && currentTime.Value.Year < year)
            {
                return BadRequest();
            }
                
            return Ok(_rentedScooterService.CalculateIncome(year, includeIncompleteRentals));
        }
    }
}
