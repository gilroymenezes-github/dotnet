using Business.Shared.Abstractions;
using System;

namespace Business.Shared.Models
{
    public class CountModel : BaseModel
    {
        public string TokenSeparatedFields { get; set; }
        public string TokenSeparatedValues { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
