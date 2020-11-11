using System;

namespace QuizManager.Shared.Models.Bson
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonDbCollectionAttribute : Attribute
    {
        public string CollectionName { get; set; }

        public BsonDbCollectionAttribute(string collectioName)
        {
            CollectionName = CollectionName;
        }
    }
}
