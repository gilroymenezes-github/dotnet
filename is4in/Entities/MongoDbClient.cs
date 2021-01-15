using IdentityServer4.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace is4in.Entities
{
    public class MongoDbClient : Client
    {
        public ObjectId Id { get; set; }
        
    }
}
