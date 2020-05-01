using System;
using System.Collections;
using System.Collections.Generic;
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
                new Jedi() { Name = "Luke", Side = "Light"},
                new Jedi() { Name = "Yoda", Side = "Light"},
                new Jedi() { Name = "Anakin", Side = "Dark"}
            };
        }
    }

    public class Jedi
    {
        public string Name {get;set;}
        public string Side {get;set;}
    }
}
