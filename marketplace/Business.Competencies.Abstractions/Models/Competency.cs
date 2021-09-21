using Business.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Competencies.Abstractions.Models
{
    public class Competency : BaseModel
    {
        public string CompetencyId { get; set; }
        public string CompetencyType { get; set; }
        public decimal HourlySeatCost { get; set; }
        public string CurrencyId { get; set; }
    }
}
