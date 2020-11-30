using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model.FundingClaims
{
    public class FundingClaimsDataExtractResultSet
    {
        public Guid SubmissionId { get; set; }

        public int CollectionId { get; set; }

        public long Ukprn { get; set; }

        public byte Declaration { get; set; }

        public byte? CovidDeclaration { get; set; }

        public byte Signed { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int SubmissionValueId { get; set; }

        public string SubmissionValueFundingStreamPeriodCode { get; set; }

        public int DeliverableCodeId { get; set; }

        public int DeliverableCode { get; set; }

        public string Description { get; set; }

        public int StudentNumbers { get; set; }

        public decimal DeliveryToDate { get; set; }

        public decimal ForecastedDelivery { get; set; }

        public decimal ExceptionalAdjustments { get; set; }

        public decimal TotalDelivery { get; set; }

        public string ContractAllocationNumber { get; set; }

        public decimal ContractValue { get; set; }
    }
}
