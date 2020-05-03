using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace HelloMongo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello Mongo!");
            var connStr = "mongodb://localhost";
            var client = new MongoClient(connStr);
            var database = client.GetDatabase("test");
            var collections = (await database.ListCollectionNamesAsync()).ToList();
            foreach(var c in collections)
            {
                Console.WriteLine($"Collection : {c}");
            }
            var collection = database.GetCollection<dynamic>("users");
            await AddRecordAsync(collection);
            await ListRecords(collection);
        }

        static async Task AddRecordAsync(IMongoCollection<dynamic> collection)
        {
            var user = new { salutation= "Mr.", firstName = "Gilroy", lastName = "Menezes" };
            await collection.InsertOneAsync(user);
        }

        static async Task ListRecords(IMongoCollection<dynamic> collection)
        {
            var filter = Builders<dynamic>.Filter.Eq("firstName", "Gilroy");
            var user = (await collection.FindAsync(filter)).FirstOrDefault();
            Console.WriteLine($"Found {user.firstName} {user.lastName}");
        }
    }
}
