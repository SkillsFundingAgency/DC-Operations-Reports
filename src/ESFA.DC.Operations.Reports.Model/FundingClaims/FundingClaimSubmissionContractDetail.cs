using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model.FundingClaims
{
    public class FundingClaimSubmissionContractDetail
    {
        public string FundingStreamPeriodCode { get; set; }

        public decimal ContractValue { get; set; }

        public Guid SubmissionId { get; set; }
    }
}
