namespace IdentityManager.Models
{
    public class RegistrationResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public RegistrationResponse(string token, string username, string email)
        {
            Token = token;
            Username = username;
            Email = email;
        }
    }
}
