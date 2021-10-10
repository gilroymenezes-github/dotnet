using Business.Core.Orders.Connections;
using Business.Core.Orders.Models;
using Business.Shared.Models;
using Business.Shared.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text.Json;

namespace Business.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class CountsController : ControllerBase
    {
        CountsTableStore countsStore;
        protected JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public CountsController(CountsTableStore countsStore)
        {
            this.countsStore = countsStore;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public async Task<IActionResult> Get()
        {
            var models = await countsStore.ReadItemsAsync("classifications");
            var groups = models.GroupBy(g => g.Name);
            var response = new List<CountModel>();
            foreach(var group in groups)
            {
                var resultDictionary = new Dictionary<string, int>() { { "A", 0 }, { "B", 0 }, { "C", 0 }, { "D", 0 }, { "E", 0 }, { "F", 0 }, { "G", 0 }, { "H", 0 } };
                group.ToList().ForEach(g =>
                {
                    var groupDictionary = JsonSerializer.Deserialize<Dictionary<string, int>>(g.JsonData, JsonSerializerOptions);
                    foreach(var kvp in resultDictionary)
                    {
                        resultDictionary[kvp.Key] += groupDictionary[kvp.Key];
                    }
                });
                response.Add(new CountModel { Name = group.Key, JsonData = JsonSerializer.Serialize(resultDictionary) });
            }
            return Ok(response);
        }

        //[HttpGet("api/[controller]/{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    return new NotFoundResult();
        //}

        //[HttpPost("api/[controller]")]
        //public async Task Post([FromBody] Order salesOrder)
        //{
        //    await commandsClient.CreateAsync(salesOrder);
        //}

        //[HttpPut("api/[controller]/{id}")]
        //public async Task Put([FromBody] Order salesOrder)
        //{
        //    await commandsClient.UpdateAsync(salesOrder);
        //}

        //[HttpDelete("api/[controller]/{id}")]
        //public async Task Delete(string id)
        //{
        //    var item = await queriesClient.GetFromIdAsync(id);
        //    item.IsDeleted = true;
        //    await commandsClient.UpdateAsync(item);
        //}
    }
}
