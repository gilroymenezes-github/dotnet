using AspNetCore.Identity.Mongo.Model;
using System;

namespace QuizManager.Admin.Models
{
    public class MongoIdentityUser : MongoUser
    {
        public string SubjectId { get; set; } = Guid.NewGuid().ToString();
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}
