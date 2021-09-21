using Business.Abstractions;
using System;
using System.Collections.Generic;

namespace Business.Projections.Abstractions.Models
{
    public class Projection : BaseModel
    {
        public string ProjectionId { get; set; }
        public DateTime ProjectionBeginDateTimeUtc { get; set; }
        public DateTime ProjectionEndDateTimeUtc { get; set; }
        public IEnumerable<string> ProjectIds { get; set; }
    }
}
