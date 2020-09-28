using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model.FundingClaims
{
    public class FundingClaimSubmissionsValue
    {
        public Guid SubmissionId { get; set; }

        public int DeliverableCodeId { get; set; }

        public decimal DeliveryToDate { get; set; }

        public decimal ForecastedDelivery { get; set; }

        public decimal ExceptionalAdjustments { get; set; }

        public decimal TotalDelivery { get; set; }

        public int StudentNumbers { get; set; }

        public string FundingStreamPeriodCode { get; set; }

        public string ContractAllocationNumber { get; set; }

        
    }
}
