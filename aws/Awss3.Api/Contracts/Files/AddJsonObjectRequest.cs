using System.Text.Json;

namespace Awss3.Api.Contracts.Files
{
    public class AddJsonObjectRequest
    {
        public Guid Id { get; set; }    
        public DateTime SentTime { get; set; }
        public string? Data { get; set; }


    }
}
