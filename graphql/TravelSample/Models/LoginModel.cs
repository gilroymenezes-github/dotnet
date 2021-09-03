using System.Text.Json;
using System.Text.Json.Serialization;

namespace TravelSample.Models
{
    public class LoginModel
    {
        [JsonPropertyName("user")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("expiry")]
        public uint Expiry { get; set; }
    }
}
