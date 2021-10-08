using Business.Shared.Abstractions;

namespace Business.Shared.Models
{
    public class FileModel : BaseModel
    {
        public string Extension { get; set; }
        public string Type { get; set; }
    }
}
