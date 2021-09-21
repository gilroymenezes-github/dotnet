using Business.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Deals.Abstractions.Models
{
    public class Category : BaseModel
    {
        public string Type { get; set; }
    }
}
