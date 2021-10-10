using Business.Shared.Abstractions;
using System;

namespace Business.Shared.Models
{
    public class CountModel : BaseModel
    {
        public DateTime PublicationDate { get; set; }
        public string JsonData { get; set; }
    }
}
