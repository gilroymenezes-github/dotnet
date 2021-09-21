using Business.Customers.Abstractions.Clients;
using Business.Customers.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        CustomersHttpClient queriesClient;

        public CustomersController(
            CustomersHttpClient queriesClient
            )
        {
            this.queriesClient = queriesClient;
        }

        [HttpGet("api/[controller]")]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await queriesClient.GetAsync();
        }

        [HttpGet("api/[controller]/{id}")]
        public async Task<Customer> Get(string id)
        {
            return await queriesClient.GetFromIdAsync(id);
        }

        //// POST api/<CustomersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<CustomersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<CustomersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
