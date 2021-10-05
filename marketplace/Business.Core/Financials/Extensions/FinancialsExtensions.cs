using Business.Core.Financials.Models;
using System;

namespace Business.Core.Financials.Extensions
{
    public static class FinancialsExtensions
    {
        public static Financial CreateFromDeal(this Financial deal, string userId)
        {
            deal.Id = Guid.NewGuid().ToString();
            deal.CreatedAtDateTimeUtc = DateTime.UtcNow;
            deal.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            deal.CreatedBy = userId;
            return deal;
        }

        public static Financial UpdateFromDeal(this Financial deal, string userId)
        {
            deal.CreatedBy ??= userId;
            deal.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            deal.UpdatedBy = userId;
            return deal;
        }

        public static Financial SoftDeleteDeal(this Financial deal, string userId)
        {
            deal.DeletedBy = userId;
            return deal;
        }
    }
}
