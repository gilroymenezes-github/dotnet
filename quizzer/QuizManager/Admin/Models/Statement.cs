using System;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Admin.Models
{
    public class Statement
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        [Required]
        public string TextValue { get; set; } = string.Empty;

        public bool? TruthValue { get; set; } = default;

        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
