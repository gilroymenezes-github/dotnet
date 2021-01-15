using AspNetCore.Identity.Mongo.Model;
using System;

namespace is4in.Entities
{
    public class MongoDbUser : MongoUser
    {
        public string SubjectId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = default;
        public string PasswordSalt { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public string ProviderSubjectId { get; set; } = string.Empty;
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public DateTime? Birthday { get; set; } 
    }
}
