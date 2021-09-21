using Business.Projections.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Projections.Abstractions.Extensions
{
    public static class ProjectionExtensions
    {
        public static Projection CreateFromProjection(this Projection projection, string userId)
        {
            projection.Id = Guid.NewGuid().ToString();
            projection.CreatedAtDateTimeUtc = DateTime.UtcNow;
            projection.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            projection.CreatedBy = userId;
            projection.AssignedTo = userId;
            return projection;
        }
        public static Projection UpdateFromProjection(this Projection projection, string userId)
        {
            projection.CreatedBy ??= userId;
            projection.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            projection.UpdatedBy = userId;
            return projection;
        }

        public static Projection SoftDeleteProjection(this Projection projection, string userId)
        {
            projection.DeletedBy = userId;
            return projection;
        }
    }
}
