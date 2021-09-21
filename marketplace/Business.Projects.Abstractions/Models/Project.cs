using Business.Abstractions;
using System;

namespace Business.Projects.Abstractions.Models
{
    public class Project : BaseModel
    {
        public string ProjectId { get; set; }
        public string ProjectTypeName { get; set; }
        public string Code { get; set; }
        public string CustomerId { get; set; }
        public bool IsActive { get; set; }
        public bool IsBillable { get; set; }
        public string BillBy { get; set; }
        public decimal HourlyRate { get; set; }
        public double Hours { get; set; }
        public decimal Cost { get; set; }
        public decimal FixedPriceValue { get; set; }
        public decimal BillableRevenueConstantCurrency { get; set; }
        public decimal BillableRevenueCurrentCurrency { get; set; }
        public DateTime ProjectBeginDateTimeUtc { get; set; }
        public DateTime ProjectEndDateTimeUtc { get; set; }
    }
}
