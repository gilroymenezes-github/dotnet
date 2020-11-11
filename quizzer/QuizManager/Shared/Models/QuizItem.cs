using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using QuizManager.Shared.Models.Bson;

namespace QuizManager.Shared.Models
{
    public class QuizItem : BsonDbDocument
    {
        public Statement Question { get; set; } = new Statement();
        public List<Statement> Answers { get; set; } = new List<Statement>();
        public bool? IsAnswerCorrect { get; set; }
        public bool IsReady { get; set; } = default;
        public bool IsPublished { get; set; } = default;
        public int QuestionScore { get; set; } = 1;
       
    }
}
