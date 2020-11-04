using System;
namespace IdentityManager.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public LoginResponse(string token, string username, string email)
        {
            Token = token;
            Username = username;
            Email = email;
        }
    }
}
