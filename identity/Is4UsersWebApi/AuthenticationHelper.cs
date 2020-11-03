using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Is4UsersWebApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Is4UsersWebApi
{
    public static class AuthenticationHelper
    {
        public static JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();
        static AuthenticationHelper()
        {


        }

        internal static string GenerateJwtToken(string email, ApplicationUser appUser, IConfiguration configuration)
        {
            var issuer = configuration["JwtIssuer"];
            var audience = configuration["JwtIssuer"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            return TokenHandler.WriteToken(new JwtSecurityToken(issuer, audience, null, null, null, signinCredentials));
        }
    }
}
