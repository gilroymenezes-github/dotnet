using AspNetCore.Identity.Mongo.Model;
using System;

namespace Identities.Models
{
    public class IdentitiesUser : MongoUser
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
