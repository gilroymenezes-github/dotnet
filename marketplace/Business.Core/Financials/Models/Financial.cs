using Business.Shared;
using Business.Shared.Abstractions;

namespace Business.Core.Financials.Models
{
    public class Financial : BaseModel
    {
        public string DealId { get; set; }
        public string ProposalId { get; set; }
        public bool IsWon { get; set; }
    }
}
