using QuizManager.Shared.Models.Bson;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Shared.Models
{
    public class Statement : BsonDbDocument
    {
        //public string Id { get; private set; } = Guid.NewGuid().ToString();
        [Required]
        public string TextValue { get; set; } = string.Empty;

        public bool? TruthValue { get; set; } = default;

        public new DateTime CreatedOn { get; set; }
    }
}
