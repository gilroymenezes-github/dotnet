using System;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Test();
        }

        static void Test() 
        {
            for(int i = 0; i < 100; ++i)
            {
                if (i % 3 == 0 && i % 5 == 0) 
                    Console.WriteLine("FizzBuzz");
                else if (i % 3 == 0)
                    Console.WriteLine("Fizz");
                else if (i % 5 == 0)
                    Console.WriteLine("Buzz");
                else 
                    Console.WriteLine(i);
            }
        }
    }
}
