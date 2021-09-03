using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelSample.Models;
using TravelSample.Services;

namespace TravelSample.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet("{description?}/{location?}")]
        public async Task<ActionResult> GetHotels(string description, string location)
        {
            var (hotels, context) = await _hotelService.FindHotel(description, location);
            return Ok(new Result(hotels, context));
        }
    }
}

