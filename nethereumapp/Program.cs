// See https://aka.ms/new-console-template for more information
//  enode://266e67a15b22e00bcd047e07a9d9a18efa1b6f5471fc1d3d9ff102cd575a9a9617f28370d38bcf4ed08ebc44b9a50b75005a049dc4efc131bdfd1e5cf88c49c1@172.17.0.2:30303
using System;
using System.Threading.Tasks;
using Nethereum.Web3;

namespace nethereumapp
{
    class Program
    {
        static void Main(string[] args)
        {
            GetBlockNumber().Wait();
        }

        static async Task GetBlockNumber()
        {
            var web3 = new Web3("enode://b1da9533924521deddae8495da187a7dbef29593207b965ec45def091ff2f0eb1e0b269f59fe22fe639c4037ca1a2c8b555e6b6c58575139e47ecd60af9428be@172.17.0.2:30303");
            var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            Console.WriteLine($"Latest Block Number is: {latestBlockNumber}");
        }
    }
}
