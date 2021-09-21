using Business.Abstractions;
using Business.ExchangeRates.Abstractions.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.ExchangeRates.Abstractions.Clients
{
    public class ExchangeRatesHttpClient : BaseClient<ExchangeRate>
    {
        public ExchangeRatesHttpClient(
            HttpClient httpClient,
            ILogger<ExchangeRatesHttpClient> logger
            ) : base(httpClient, logger)
        {
            ResourceName = "exchangerates";
        }

        public override Task AddItemAsync(ExchangeRate item) => Task.CompletedTask;

        public override Task DeleteItemAsync(string id, bool isHardDelete = false) => Task.CompletedTask;

        public override Task EditItemAsync(ExchangeRate item) => Task.CompletedTask;
               
    }
}
