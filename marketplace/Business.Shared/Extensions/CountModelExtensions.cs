using Business.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Shared.Extensions
{
    public static class CountModelExtensions
    {
        public static CountModel CreateFromCountModel(this CountModel model, string name, DateTime date)
        {
            model.Id = Guid.NewGuid().ToString();
            model.CreatedAtDateTimeUtc = DateTime.UtcNow;
            model.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            model.CountAtDateTimeUtc = date.ToUniversalTime();
            model.Name = name;
            model.TsvToken = ";";
            return model;
        }
    }
}
