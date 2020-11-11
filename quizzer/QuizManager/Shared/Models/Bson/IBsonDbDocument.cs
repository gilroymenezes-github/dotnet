using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizManager.Shared.Models.Bson
{
    public interface IBsonDbDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedOn { get; }

        DateTime? UpdatedOn { get; set; }
    }

    public abstract class BsonDbDocument : IBsonDbDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedOn => Id.CreationTime;

        public DateTime? UpdatedOn { get; set; }
    }
}
