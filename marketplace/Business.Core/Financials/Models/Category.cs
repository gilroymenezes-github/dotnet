using Business.Shared;
using Business.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Core.Financials.Models
{
    public class Category : BaseModel
    {
        public string Type { get; set; }
    }
}
