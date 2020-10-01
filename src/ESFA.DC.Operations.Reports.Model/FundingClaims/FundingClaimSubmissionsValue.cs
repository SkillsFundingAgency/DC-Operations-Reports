using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model.FundingClaims
{
    public class FundingClaimSubmissionsValue
    {
        public Guid SubmissionId { get; set; }

        public decimal TotalDelivery { get; set; }

        public string FundingStreamPeriodCode { get; set; }

        public string ContractAllocationNumber { get; set; }
    }
}
