using Microsoft.AspNetCore.Mvc;
using ScooterRental.Core.Interfaces;
using ScooterRental.Core.Models;
using ScooterRental.Core.Services;

namespace ScooterRental_Api.Controllers
{
    [Route("scooter-api")]
    [ApiController]
    public class ScooterController : ControllerBase
    {
        private readonly IEntityService<Scooter> _entityService;
        private readonly IEnumerable<IScooterValidator> _scooterValidator;

        public ScooterController(IEntityService<Scooter> entityService, IEnumerable<IScooterValidator> scooterValidator)
        {
            _entityService = entityService;
            _scooterValidator = scooterValidator;
        }

        [HttpPut]
        [Route("add/scooter")]
        public IActionResult AddScooter(Scooter scooter)
        {
            if (_scooterValidator.Any(x => !x.IsValid(scooter))) 
            {
                return BadRequest();
            }

            if (_entityService.GetById(scooter.Id) != null) 
            {
                return Conflict(scooter.Id);
            }
            
            var result = _entityService.Create(scooter);

            return Created("", scooter);
        }

        [HttpDelete]
        [Route("delete/scooter/{id}")]
        public IActionResult DeleteScooter(string id)
        {
            var scooter = _entityService.GetById(id);

            if (scooter == null) 
            {
                return NotFound();
            }
            
            _entityService.Delete(scooter);

            return Ok();
        }

        [HttpGet]
        [Route("get/scooter/{id}")]
        public IActionResult GetScooter(string id)
        {
            var scooter = _entityService.GetById(id);

            if (scooter == null) 
            {
                return NotFound($"Scooter id {id} does not exist");
            }           
            return Ok(scooter);
        }

        [HttpGet]
        [Route("get/all/scooters")]
        public IActionResult GetAllScooters()
        {
            var scooters = _entityService.GetAll();

            if (!scooters.Any()) 
            {
                return NoContent();
            } 
            return Ok(scooters);
        }
    }
}
