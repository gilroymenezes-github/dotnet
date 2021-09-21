using Business.Abstractions;

namespace Business.ExchangeRates.Abstractions.Models
{
    public class Currency : BaseModel
    {
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
    }
}
