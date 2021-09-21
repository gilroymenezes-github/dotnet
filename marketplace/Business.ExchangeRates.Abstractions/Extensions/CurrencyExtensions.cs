using Business.ExchangeRates.Abstractions.Models;
using System;

namespace Business.ExchangeRates.Abstractions.Extensions
{
    public static class CurrencyExtensions 
    {
        public static Currency CreateFromCurrency(this Currency currency, string userId)
        {
            currency.Id = Guid.NewGuid().ToString();
            currency.CreatedBy = userId;
            currency.CreatedAtDateTimeUtc = DateTime.UtcNow;
            currency.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            return currency;
        }

        public static Currency UpdateFromCurrency(this Currency currency, string userId)
        {
            currency.CreatedBy ??= userId;
            currency.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            currency.UpdatedBy = userId;
            return currency;
        }

        public static Currency SoftDeleteCurrency(this Currency currency, string userId)
        {
            currency.DeletedBy = userId;
            return currency;
        }

        public static Currency EmptyCurrency(this Currency currency)
        {
            currency.Id = string.Empty;
            currency.CurrencyId = string.Empty;
            return currency;
        }
    }
}
