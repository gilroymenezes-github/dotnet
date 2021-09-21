

using Business.Abstractions;

namespace Business.SalesOrders.Abstractions.Models
{
    public class SalesOrder : BaseModel
    {
        public string SalesOrderId { get; set; }
        public string DealId { get; set; }
    }
}
