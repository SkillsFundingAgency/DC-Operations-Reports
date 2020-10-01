using ESFA.DC.Operations.Reports.Model.FundingClaims;
using System;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders
{
    public class FundingClaimSubmissionContractDetailBuilder : AbstractBuilder<FundingClaimSubmissionContractDetail>
    {
        public Guid SubmissionId = new Guid("08BD2CBD-FB97-447D-860A-FEAB8D03A5EA");

        public decimal ContractValue = 11.0M;

        public const string FundingStreamPeriodCode = "ALLBC1920";

        public FundingClaimSubmissionContractDetailBuilder()
        {
            modelObject = new FundingClaimSubmissionContractDetail()
            {
                SubmissionId = SubmissionId,
                ContractValue = ContractValue,
                FundingStreamPeriodCode = FundingStreamPeriodCode
            };
        }
    }
}
