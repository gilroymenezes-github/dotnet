using System;
using AspNetCore.Identity.Mongo.Model;

namespace Is4UsersWebApi.Models
{
    public class ApplicationUser : MongoUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}
