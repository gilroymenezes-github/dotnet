using Business.Customers.Abstractions.Models;
using Business.ExchangeRates.Abstractions.Models;

namespace Business.Projects.Abstractions.Models
{
    public class ProjectViewModel
    {
        public Project Project { get; set; }
        public Customer Customer { get; set; }
        public Currency Currency { get; set; }
    }
}
