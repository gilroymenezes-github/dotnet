using Business.Abstractions;

namespace Business.ExchangeRates.Abstractions.Models
{

    public class ExchangeRate : BaseModel
    {
        public string ExchangeRateId { get; set; }
        public string SourceCurrencyId { get; set; }
        public string TargetCurrencyId { get; set; }
        public decimal ExchangeRateValue { get; set; }
        public decimal WeightedFactorValue { get; set; }
    }

  
}
