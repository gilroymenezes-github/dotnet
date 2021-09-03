using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TravelSample.Models
{
    public class BookFlightModel
    {
        [JsonPropertyName("flights")]
        public List<Flight> Flights { get; set; }
    }
}
