using Business.Core.Financials.Connections;
using Business.Core.Financials.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class DealsController : ControllerBase
    {
        FinancialsHttpClient queriesClient;
        FinancialsQueueClient commandsClient;
        public DealsController(FinancialsHttpClient queriesClient, FinancialsQueueClient commandsClient)
        {
            this.queriesClient = queriesClient;
            this.commandsClient = commandsClient;

        }

        [HttpGet("api/[controller]")]
        public async Task<IEnumerable<Financial>> Get()
        {
            return await queriesClient.GetAsync();
        }

        [HttpGet("api/[controller]/{id}")]
        public async Task<Financial> Get(string id)
        {
            return await queriesClient.GetFromIdAsync(id);
        }

        [HttpPost("api/[controller]")]
        public async Task Post([FromBody] Financial deal)
        {
            await commandsClient.CreateAsync(deal);
        }

        [HttpPut("api/[controller]/{id}")]
        public async Task Put([FromBody] Financial deal)
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
