using Infrastructure.Abstractions;

namespace Infrastructure.Models
{
    public class FileEntity : BaseEntity
    {
        public string Extension { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
    }
}
