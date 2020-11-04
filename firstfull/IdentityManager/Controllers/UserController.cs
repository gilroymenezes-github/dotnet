using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityManager.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentitiesUser> _userManager;
        private readonly SignInManager<IdentitiesUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserController(UserManager<IdentitiesUser> userManager, SignInManager<IdentitiesUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        //GET api/v1/user/userinfo
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult> UserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            var userInfo = new UserInfo
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Email = user.Email
            };
            return Ok(userInfo);
        }

        //POST api/v1/user/register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegistrationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentitiesUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    City = model.City,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    var token = TokenService.GenerateJwtToken(model.Email, user, _configuration);
                    var rootData = new RegistrationResponse(token, user.UserName, user.Email);
                    return Created("api/v1/authentication/register", rootData);
                }
                return Ok(string.Join(",", result.Errors?.Select(error => error.Description)));
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }

        //POST api/v1/user/login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody]LoginUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    var token = TokenService.GenerateJwtToken(model.Email, appUser, _configuration);
                    var rootData = new LoginResponse(token, appUser.UserName, appUser.Email);
                    return Ok(rootData);
                }
                return StatusCode((int)HttpStatusCode.Unauthorized, "Bad Credentials");
            }
            string errorMessage = string.Join(", ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return BadRequest(errorMessage ?? "Bad Request");
        }
    }
}
