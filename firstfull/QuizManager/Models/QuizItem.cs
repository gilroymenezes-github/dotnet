using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class QuizItem
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        [Required]
        public Statement Question { get; set; } = new Statement();
        public List<Statement> Answers { get; set; } = new List<Statement>();
        public bool? IsAnswerCorrect { get; set; }
        public bool IsReady { get; set; } = default;
        public bool IsPublished { get; set; } = default;
        public int QuestionScore { get; set; } = 1;
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
