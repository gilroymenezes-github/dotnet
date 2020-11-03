using System;
namespace Is4UsersWebApi.Models
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

    public class SignUpResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public SignUpResponse(string token, string username, string email)
        {
            Token = token;
            Username = username;
            Email = email;
        }
    }

    public class UserInfoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
    }
}
