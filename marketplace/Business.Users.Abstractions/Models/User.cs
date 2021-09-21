using Business.Abstractions;
using System;

namespace Business.Users.Abstractions.Models
{
    public class User : BaseModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UnitName { get; set; }
        public string RoleName { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
    }
}
