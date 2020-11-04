using QuizManager.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizManager.Services
{
    public class QuizService
    {
        private static readonly List<QuizItem> Quiz;

        static QuizService()
        {
            Quiz = new List<QuizItem> {
                new QuizItem
                {
                    Question = new Statement { TextValue = "Which of the following is the name of a Leonardo da Vinci's masterpiece?", TruthValue = null },
                    Answers = new List<Statement>
                    {
                        new Statement { TextValue = "Sunflowers", TruthValue = default },
                        new Statement { TextValue = "Mona Lisa", TruthValue = true },
                        new Statement { TextValue = "The Kiss", TruthValue = default},
                    },
                    IsAnswerCorrect = null,
                    QuestionScore = 3
                },
                new QuizItem
                {
                    Question = new Statement { TextValue =  "Which of the following novels was written by Miguel de Cervantes?", TruthValue = null },
                    Answers = new List<Statement>
                    {
                        new Statement { TextValue = "The Ingenious Gentleman Don Quixote of La Mancia", TruthValue = true },
                        new Statement { TextValue = "The Life of Gargantua and of Pantagruel", TruthValue = default },
                        new Statement { TextValue = "One Hundred Years of Solitude", TruthValue = default }
                    },
                    IsAnswerCorrect = null,
                    QuestionScore = 5
                }
            };
        }

        public Task<List<QuizItem>> GetQuizAsync()
        {
            return Task.FromResult(Quiz);
        }
    }
}
