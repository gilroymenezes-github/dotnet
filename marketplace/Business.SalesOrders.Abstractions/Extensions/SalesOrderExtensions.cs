using Business.SalesOrders.Abstractions.Models;
using System;

namespace Business.SalesOrders.Abstractions.Extensions
{
    public static class SalesOrderExtensions
    {
        public static SalesOrder CreateFromSalesOrder(this SalesOrder salesOrder, string userId)
        {
            salesOrder.Id = Guid.NewGuid().ToString();
            salesOrder.CreatedAtDateTimeUtc = DateTime.UtcNow;
            salesOrder.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            salesOrder.CreatedBy = userId;
            return salesOrder;
        }

        public static SalesOrder UpdateFromSalesOrder(this SalesOrder salesOrder, string userId)
        {
            salesOrder.CreatedBy ??= userId;
            salesOrder.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            salesOrder.UpdatedBy = userId;
            return salesOrder;
        }

        public static SalesOrder SoftDeleteSalesOrder(this SalesOrder salesOrder, string userId)
        {
            salesOrder.DeletedBy = userId;
            return salesOrder;
        }
    }
}
