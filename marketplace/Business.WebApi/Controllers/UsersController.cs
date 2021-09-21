using Business.Users.Abstractions.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UsersHttpClient queriesClient;
        
        public UsersController(UsersHttpClient queriesClient)
        {
            this.queriesClient = queriesClient;
        }

        [HttpGet("api/[controller]")]
        public async Task<ActionResult<IAsyncEnumerable<Users.Abstractions.Models.User>>> Get()
        {
            var users = await queriesClient.GetAsync();
            if (users is null) return NotFound("No records");
            else return Ok(users);
        }

        [HttpGet("api/[controller]/{id}")]
        public async Task<ActionResult<Users.Abstractions.Models.User>> Get(string userId)
        {
            var users = await queriesClient.GetAsync();
            var u = users.FirstOrDefault(f => f.Email == "user@email.in"); // hard-coded to remove
            return Ok(u);
            //var user = await queriesClient.GetFromIdAsync(userId);
            //if (user is null) return NotFound("No records");
            //else return Ok(user);
        }

        //// POST api/<UsersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UsersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
