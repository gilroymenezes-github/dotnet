using Business.Deals.Abstractions.Models;
using System;

namespace Business.Deals.Abstractions.Extensions
{
    public static class DealExtensions
    {
        public static Deal CreateFromDeal(this Deal deal, string userId)
        {
            deal.Id = Guid.NewGuid().ToString();
            deal.CreatedAtDateTimeUtc = DateTime.UtcNow;
            deal.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            deal.CreatedBy = userId;
            return deal;
        }

        public static Deal UpdateFromDeal(this Deal deal, string userId)
        {
            deal.CreatedBy ??= userId;
            deal.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            deal.UpdatedBy = userId;
            return deal;
        }

        public static Deal SoftDeleteDeal(this Deal deal, string userId)
        {
            deal.DeletedBy = userId;
            return deal;
        }
    }
}
