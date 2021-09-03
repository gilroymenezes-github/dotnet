using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelSample.Models;
using TravelSample.Services;

namespace TravelSample.Controllers
{
    [ApiController]
    [Route("api/airports")]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportsService _airportService;

        public AirportsController(IAirportsService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetFlights(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest("Missing / invalid arguments.");
            }

            var (routes, context) = await _airportService.GetAirports(search);
            return Ok(new Result(routes, context));
        }
    }
}
