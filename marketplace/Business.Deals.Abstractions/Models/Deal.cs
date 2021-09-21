using Business.Abstractions;

namespace Business.Deals.Abstractions.Models
{
    public class Deal : BaseModel
    {
        public string DealId { get; set; }
        public string ProposalId { get; set; }
        public bool IsWon { get; set; }
    }
}
