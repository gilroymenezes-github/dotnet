using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Business.WebApp.Dashboards.GitHub.Models
{
    public class User
    {
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; }
    }
}
