using Business.Core.Orders.Models;
using System;

namespace Business.Core.Orders.Extensions
{
    public static class OrdersExtensions
    {
        public static Order CreateFromSalesOrder(this Order salesOrder, string userId)
        {
            salesOrder.Id = Guid.NewGuid().ToString();
            salesOrder.CreatedAtDateTimeUtc = DateTime.UtcNow;
            salesOrder.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            salesOrder.CreatedBy = userId;
            return salesOrder;
        }

        public static Order UpdateFromSalesOrder(this Order salesOrder, string userId)
        {
            salesOrder.CreatedBy ??= userId;
            salesOrder.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            salesOrder.UpdatedBy = userId;
            return salesOrder;
        }

        public static Order SoftDeleteSalesOrder(this Order salesOrder, string userId)
        {
            salesOrder.DeletedBy = userId;
            return salesOrder;
        }
    }
}
