using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Models
{
    public class QuizItem
    {
        public Statement Question { get; set; }
        public List<Statement> Answers { get; set; }
        public bool? IsAnswerCorrect { get; set; }
        public int QuestionScore { get; set; }
    }
}
