using Business.Deals.Abstractions.Clients;
using Business.Deals.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.ExchangeRates.Controllers
{
    [Authorize]
    [ApiController]
    public class DealsController : ControllerBase
    {
        DealsHttpClient queriesClient;
        DealsQueueClient commandsClient;
        public DealsController(DealsHttpClient queriesClient, DealsQueueClient commandsClient)
        {
            this.queriesClient = queriesClient;
            this.commandsClient = commandsClient;

        }

        [HttpGet("api/[controller]")]
        public async Task<IEnumerable<Deal>> Get()
        {
            return await queriesClient.GetAsync();
        }

        [HttpGet("api/[controller]/{id}")]
        public async Task<Deal> Get(string id)
        {
            return await queriesClient.GetFromIdAsync(id);
        }

        [HttpPost("api/[controller]")]
        public async Task Post([FromBody] Deal deal)
        {
            await commandsClient.CreateAsync(deal);
        }

        [HttpPut("api/[controller]/{id}")]
        public async Task Put([FromBody] Deal deal)
        {
            await commandsClient.UpdateAsync(deal);
        }

        //[HttpDelete("api/[controller]/{id}")]
        //public async Task Delete(string id)
        //{
        //    var item = await queriesClient.GetFromIdAsync(id);
        //    item.IsDeleted = true;
        //    await commandsClient.UpdateAsync(item);
        //}
    }
}
