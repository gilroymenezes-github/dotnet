using Business.Shared;

namespace Business.Core.Customers.Models
{
    public class Customer : BaseModel
    {
        public string CustomerId { get; set; }
        public string Address { get; set; }
        public string CurrencyId { get; set; }
        public bool IsActive { get; set; }
    }
}
