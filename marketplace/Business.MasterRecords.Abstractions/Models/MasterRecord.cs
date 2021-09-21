using Business.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MasterRecords.Abstractions.Models
{
    public class MasterRecord : BaseModel
    {
        public string MasterRecordId { get; set; }
        public string UnitId { get; set; }
        public string CompetencyId { get; set; }
    }
}
