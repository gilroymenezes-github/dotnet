using Business.Shared.Models;
using System;
using System.Globalization;

namespace Business.Shared.Extensions
{
    public static class CountModelExtensions
    {
        public static CountModel CreateFromCountModel(this CountModel model)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedAtDateTimeUtc = DateTime.UtcNow;
            model.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            model.TsvToken = ";";
            return model;
        }
    }
}
