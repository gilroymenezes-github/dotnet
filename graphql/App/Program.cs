using System;
using GraphQL;
using GraphQL.Types;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            var schema = Schema.For(@"
            type Jedi {
                name: String,
                side: String
            }
            type Query {
                hello: String,
                jedis: [Jedi]
            }",
            _=> 
            {
                _.Types.Include<Query>();
            });
            var root = new { Hello = "Hello World!"};
            var json = schema.Execute( _ => 
            {
                // _.Query = "{ hello }";
                // _.Root = root;
                _.Query = "{ jedis { name, side }}";
            });

            Console.WriteLine(json);
        }
    }
}
