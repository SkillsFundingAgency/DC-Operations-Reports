using ESFA.DC.Operations.Reports.Model.FundingClaims;
using System;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders
{
    public class FundingClaimSubmissionsValueBuilder : AbstractBuilder<FundingClaimSubmissionsValue>
    {

        public decimal TotalDelivery = 10.0M;

        public const string FundingStreamPeriodCode = "ALLBC1920";

        public const string ContractAllocationNumber = "ALLC-4391";

        public FundingClaimSubmissionsValueBuilder()
        {
            modelObject = new FundingClaimSubmissionsValue()
            {
                TotalDelivery = TotalDelivery,
                FundingStreamPeriodCode = FundingStreamPeriodCode,
                ContractAllocationNumber = ContractAllocationNumber
            };
        }
    }
}
