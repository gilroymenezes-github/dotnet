using Business.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Deals.Abstractions.Models
{
    public abstract class Inventory : BaseModel
    {
        public string SkuId { get; set; }
        public string PriceId { get; set; }
        public string SupplierId { get; set; }
        public string ImageUrl { get; set; }
    }
}
