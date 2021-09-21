using Business.Competencies.Abstractions.Models;
using System;

namespace Business.Competencies.Abstractions.Extensions
{
    public static class CompetencyExtensions
    {
        public static Competency CreateFromCompetency(this Competency competency, string userId)
        {
            competency.Id = Guid.NewGuid().ToString();
            competency.CreatedAtDateTimeUtc = DateTime.UtcNow;
            competency.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            competency.CreatedBy = userId;
            return competency;
        }

        public static Competency UpdateFromCompetency(this Competency competency, string userId)
        {
            competency.CreatedBy ??= userId;
            competency.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            competency.UpdatedBy = userId;
            return competency;
        }

        public static Competency SoftDeleteCompetency(this Competency competency, string userId)
        {
            competency.DeletedBy = userId;
            return competency;
        }
    }
}

