using ESFA.DC.Operations.Reports.Model.FundingClaims;
using ESFA.DC.Operations.Reports.Reports.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders
{
    public class FundingClaimsSubmissionBuilder : AbstractBuilder<FundingClaimsSubmission>
    {
        public Guid SubmissionId = new Guid("08BD2CBD-FB97-447D-860A-FEAB8D03A5EA");

        public const bool IsSubmitted = true;

        public const int CollectionId = 174;

        public int Ukprn = 12345678;

        public FundingClaimsSubmissionBuilder(Guid? submissionIdOverride = null)
        {
            var submissionId = submissionIdOverride ?? SubmissionId;
            modelObject = new FundingClaimsSubmission()
            {
                SubmissionId = submissionId,
                IsSubmitted = IsSubmitted,
                CollectionId = CollectionId,
                Ukprn = Ukprn,
                SubmissionValues = new List<FundingClaimSubmissionsValue>
                {
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.ALLBC1920).With(x => x.TotalDelivery, 10).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBCASCL1920).With(x => x.TotalDelivery, 20).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBC19TRN1920).With(x => x.TotalDelivery, 30).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBASLS1920).With(x => x.TotalDelivery, 40).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEB19TRLS1920).With(x => x.TotalDelivery, 50).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.C1619ED1920).With(x => x.TotalDelivery, 60).With(x => x.SubmissionId, submissionId).Build(),
                },

                SubmissionContractDetails = new List<FundingClaimSubmissionContractDetail>
                {
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.ALLBC1920).With(x => x.ContractValue, 11).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBCASCL1920).With(x => x.ContractValue, 21).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBC19TRN1920).With(x => x.ContractValue, 31).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBASLS1920).With(x => x.ContractValue, 41).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEB19TRLS1920).With(x => x.ContractValue, 51).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.C1619ED1920).With(x => x.ContractValue, 61).With(x => x.SubmissionId, submissionId).Build(),
                },
            };
        }
    }
}
