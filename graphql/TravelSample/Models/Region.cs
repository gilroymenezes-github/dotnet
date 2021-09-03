using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelSample.Models
{
    public class Region
    {
        public IList<Airport> Airports { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
