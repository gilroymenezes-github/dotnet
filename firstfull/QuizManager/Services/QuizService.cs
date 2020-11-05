using MongoDB.Driver;
using QuizManager.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace QuizManager.Services
{
    public class QuizService
    {
        public async Task<List<QuizItem>> GetQuizAsync()
        {
            var mongoClient = new MongoClient("mongodb://192.168.0.105:27017");
            var database = mongoClient.GetDatabase("quizzes");
            var collection = database.GetCollection<QuizItem>("Items");
            var items = await collection.Find(_ => true).ToListAsync();

            if (items.Count == 0)
            {
                // fill from mock data..
                foreach (var item in QuizItems)
                {
                    await collection.InsertOneAsync(item);
                }
            }
            return await Task.FromResult(QuizItems);
        }

        #region mock data
        private static readonly List<QuizItem> QuizItems;

        static QuizService()
        {
            QuizItems = new List<QuizItem> {
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
        #endregion

    }
}
