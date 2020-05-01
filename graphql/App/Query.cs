using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;

namespace App
{
    public class Query
    {
        [GraphQLMetadata("jedis")]
        public IEnumerable<Jedi> GetJedis()
        {
            return StarWarsDb.GetJedis();
        }
        [GraphQLMetadata("jedi")]
        public Jedi GetJedi(int id)
        {
            return StarWarsDb.GetJedis().SingleOrDefault(j => j.Id == id);
        }

        [GraphQLMetadata("hello")]
        public string GetHello()
        {
            return "Hello Query Class!";
        }
        
    }
    
    public static class StarWarsDb
    {
        public static IEnumerable<Jedi> GetJedis()
        {
            return new List<Jedi>() {
                new Jedi() { Id = 1, Name = "Luke", Side = "Light"},
                new Jedi() { Id = 2, Name = "Yoda", Side = "Light"},
                new Jedi() { Id = 3, Name = "Anakin", Side = "Dark"}
            };
        }
    }

    public class Jedi
    {
        public int Id {get;set;}
        public string Name {get;set;}
        public string Side {get;set;}
    }
}
