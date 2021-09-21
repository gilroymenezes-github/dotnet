using Business.ExchangeRates.Abstractions.Models;
using System;

namespace Business.ExchangeRates.Abstractions.Extensions
{
    public static class ExchangeRateExtensions
    {
        public static ExchangeRate CreateFromExchangeRate(this ExchangeRate exchangerate, string userId)
        {
            exchangerate.Id = Guid.NewGuid().ToString();
            exchangerate.CreatedBy = userId;
            exchangerate.CreatedAtDateTimeUtc = DateTime.UtcNow;
            exchangerate.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            return exchangerate;
        }

        public static ExchangeRate UpdateFromExchangeRate(this ExchangeRate exchangerate, string userId)
        {
            exchangerate.CreatedBy ??= userId;
            exchangerate.UpdatedBy = userId;
            exchangerate.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            return exchangerate;
        }

        public static ExchangeRate SoftDeleteExchangeRate(this ExchangeRate currencyConversion, string userId)
        {
            currencyConversion.DeletedBy = userId;
            return currencyConversion;
        }
    }
}
