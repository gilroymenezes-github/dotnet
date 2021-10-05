

using Business.Shared;

namespace Business.Core.Orders.Models
{
    public class Order : BaseModel
    {
        public string SalesOrderId { get; set; }
        public string DealId { get; set; }
    }
}
