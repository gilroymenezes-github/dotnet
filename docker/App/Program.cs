using System;
using System.Threading.Tasks;

namespace NetCore.Docker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            var counter = 0;
            int v = args.Length != 0
                        ? Convert.ToInt32(args[0])
                        : -1;
            while (v == -1 || counter < v)
            {
                Console.WriteLine($"Counter: {++counter}");
                await Task.Delay(1000);
            }
        }
    }
}
