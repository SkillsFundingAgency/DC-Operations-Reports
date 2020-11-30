using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model
{
    public class FundingClaimsDataExtractDetail
    {
        public Guid SubmissionId { get; set; }

        public long Ukprn { get; set; }

        public string CollectionPeriod { get; set; }

        public string ProviderName { get; set; }

        public string UpdatedOn { get; set; }

        public byte Declaration { get; set; }

        public byte? CovidDeclaration { get; set; }

        public byte Signed { get; set; }

        public string FundingStreamPeriodCode { get; set; }

        public decimal MaximumContractValue { get; set; }

        public int DeliverableCode { get; set; }

        public string DeliverableDescription { get; set; }

        public int StudentNumbers { get; set; }

        public decimal DeliveryToDate { get; set; }

        public decimal ForecastedDelivery { get; set; }

        public decimal ExceptionalAdjustments { get; set; }

        public decimal TotalDelivery { get; set; }

        public string ContractAllocationNumber { get; set; }
    }
}
