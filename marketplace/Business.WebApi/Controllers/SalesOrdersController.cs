using Business.Core.Orders.Connections;
using Business.Core.Orders.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class SalesOrdersController : ControllerBase
    {
        OrdersHttpClient queriesClient;
        OrdersQueueClient commandsClient;

        public SalesOrdersController(OrdersHttpClient queriesClient, OrdersQueueClient commandsClient)
        {
            this.queriesClient = queriesClient;
            this.commandsClient = commandsClient;
        }

        [HttpGet("api/[controller]")]
        public async Task<IEnumerable<Order>> Get()
        {
            return await queriesClient.GetAsync();
        }

        [HttpGet("api/[controller]/{id}")]
        public async Task<Order> Get(string id)
        {
            return await queriesClient.GetFromIdAsync(id);
        }

        [HttpPost("api/[controller]")]
        public async Task Post([FromBody] Order salesOrder)
        {
            await commandsClient.CreateAsync(salesOrder);
        }

        [HttpPut("api/[controller]/{id}")]
        public async Task Put([FromBody] Order salesOrder)
        {
            await commandsClient.UpdateAsync(salesOrder);
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
