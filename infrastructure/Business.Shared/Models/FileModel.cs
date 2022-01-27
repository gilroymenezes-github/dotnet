using Infrastructure.Abstractions;

namespace Infrastructure.Models
{
    public class FileModel : BaseModel
    {
        public string Extension { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
    }
}
