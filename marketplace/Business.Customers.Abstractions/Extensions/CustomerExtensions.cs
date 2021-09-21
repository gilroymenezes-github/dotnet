using Business.Customers.Abstractions.Models;
using System;

namespace Business.Customers.Abstractions.Extensions
{
    public static class CustomerExtensions
    {
        public static Customer UpdateFromCustomer(this Customer customer)
        {
            customer.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            customer.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            return customer;
        }

        public static Customer EmptyCustomer(this Customer customer)
        {
            customer.Id = string.Empty;
            customer.CustomerId = string.Empty;
            customer.CurrencyId = string.Empty;
            return customer;
        }
    }
}
