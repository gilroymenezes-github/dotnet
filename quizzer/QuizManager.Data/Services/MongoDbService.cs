﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using QuizManager.Shared.Models;

namespace QuizManager.Data.Services
{
    public class MongoDbService
    {

        private const string _collectionName = "Items";
        private const string _databaseName = "quizzes";
        private IMongoDatabase _database;

        public MongoDbService()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            _database = mongoClient.GetDatabase(_databaseName);
        }

        public async Task InsertQuizItem(QuizItem quizItem)
        {
            var collection = _database.GetCollection<QuizItem>(_collectionName);
            await collection.InsertOneAsync(quizItem);
        }

        public async Task DeleteQuizItem(QuizItem quizItem)
        {
            var deleteFilter = Builders<QuizItem>.Filter.Eq("_id", quizItem.Id);
            var collection = _database.GetCollection<QuizItem>(_collectionName);
            await collection.DeleteOneAsync(deleteFilter);
        }

        public async Task<List<QuizItem>> GetQuizItemsAsync()
        {
            var collection = _database.GetCollection<QuizItem>(_collectionName);
            var items = await collection.Find(_ => true).ToListAsync();

            if (items.Count == 0)
            {
                // fill from mock data..
                foreach (var quizItem in QuizItems)
                {
                    await collection.InsertOneAsync(quizItem);
                }
            }
            return items;
            //return await Task.FromResult(QuizItems);
        }

        #region mock data
        private static readonly List<QuizItem> QuizItems;

        static MongoDbService()
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
