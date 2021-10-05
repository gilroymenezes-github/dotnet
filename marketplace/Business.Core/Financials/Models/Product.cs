using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Core.Financials.Models
{
    public class Product : Inventory
    {
        public string ProductId { get; set; }
        public string Upc { get; set; }
        public string LocationId { get; set; }
        public string CategoriesTsv { get; set; }
    }
}
