using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizManager.Models
{
    public class Statement
    {
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public string TextValue { get; set; }
        public bool? TruthValue { get; set; }
    }
}
