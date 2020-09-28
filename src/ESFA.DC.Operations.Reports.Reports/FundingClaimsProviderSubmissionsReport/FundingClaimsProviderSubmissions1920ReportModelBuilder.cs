using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.FundingClaims;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Operations.Reports.Reports.Extensions;

namespace ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport
{
    public class FundingClaimsProviderSubmissions1920ReportModelBuilder : IFundingClaimsSubmissionsModelBuilder
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public FundingClaimsProviderSubmissions1920ReportModelBuilder(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<FundingClaimsSubmissionsModel> Build(CollectionDetail collectionDetail,
            IEnumerable<OrganisationCollection> expectedProviders,
            IEnumerable<FundingClaimsSubmission> fundingClaimsSubmissions,
            IDictionary<int, OrgModel> orgDetails,
            CancellationToken cancellationToken)
        {

            var model = new FundingClaimsSubmissionsModel();
            var expectedProvidersList = expectedProviders.ToList();
            var fundingClaimsSubmissionsList = fundingClaimsSubmissions.ToList();
            var submittedProviderUkprns = fundingClaimsSubmissionsList.Select(x => x.Ukprn).ToList();
            var expectedUkprns = expectedProvidersList.Select(x => (long)x.Ukprn).ToList();

            var expectedProvidersNotSubmitted = expectedProvidersList.Where(x => !submittedProviderUkprns.Contains(x.Ukprn)).ToList();
            var expectedProvidersSubmitted = expectedProvidersList.Where(x => submittedProviderUkprns.Contains(x.Ukprn)).ToList();

            var unexpectedReturningProviders = submittedProviderUkprns.Where(x => !expectedUkprns.Contains(x)).ToList();

            var totalProviders = expectedUkprns.Union(submittedProviderUkprns).Count();

            model.FundingClaim = collectionDetail.DisplayTitle;
            model.ReportRun = _dateTimeProvider.GetNowUtc().LongDateStringFormat();
            model.TotalNoOfProviders = totalProviders;
            model.NoOfProvidersExpectedToReturn = expectedUkprns.Count;
            model.NoOfReturningExpectedProviders = expectedProvidersSubmitted.Count;
            model.NoOfExpectedProvidersNotReturning = expectedProvidersNotSubmitted.Count;
            model.NoOfReturningUnexpectedProviders = unexpectedReturningProviders.Count;


            var submissionsDetails = new List<FundingClaimsSubmissionsDetail>();
            foreach (var submission in fundingClaimsSubmissionsList)
            {
                var detail = new FundingClaimsSubmissionsDetail
                {
                    UkPrn = submission.Ukprn,
                    ProviderName = orgDetails.GetValueOrDefault((int)submission.Ukprn)?.Name,
                    ExpectedToReturnInCurrentPeriod = isExpectedToReturn(submission.Ukprn, expectedProvidersList),
                    ReturnedInCurrentPeriod = submission.IsSubmitted ? "Yes" : "No",
                    DateLatestClaimSubmitted = submission.SubmittedDateTimeUtc?.LongDateStringFormat(),
                };

                detail.ALLBC1920ContractValue = GetContractValue(submission.SubmissionId, "ALLBC1920", submission.SubmissionContractDetails);
                detail.ALLBC1920Claimed = GetClaimedValue(submission.SubmissionId, "ALLBC1920", submission.SubmissionValues);

                detail.AEBCASCL1920ContractValue = GetContractValue(submission.SubmissionId, "AEBC-ASCL1920", submission.SubmissionContractDetails);
                detail.AEBCASCL1920Claimed = GetClaimedValue(submission.SubmissionId, "AEBC-ASCL1920", submission.SubmissionValues);

                detail.AEBC19TRN1920ContractValue = GetContractValue(submission.SubmissionId, "AEBC-19TRN1920", submission.SubmissionContractDetails);
                detail.AEBC19TRN1920Claimed = GetClaimedValue(submission.SubmissionId, "AEBC-19TRN1920", submission.SubmissionValues);

                detail.AEBASLS1920ProcuredContractValue = GetContractValue(submission.SubmissionId, "AEB-ASLS1920", submission.SubmissionContractDetails);
                detail.AEBASLS1920ProcuredClaimed = GetClaimedValue(submission.SubmissionId, "AEB-ASLS1920", submission.SubmissionValues);

                detail.AEB19TRLS1920ProcuredContractValue = GetContractValue(submission.SubmissionId, "AEB-19TRLS1920", submission.SubmissionContractDetails);
                detail.AEB19TRLS1920ProcuredClaimed = GetClaimedValue(submission.SubmissionId, "AEB-19TRLS1920", submission.SubmissionValues);

                detail.ED1920ContractValue1619 = GetContractValue(submission.SubmissionId, "1619ED1920", submission.SubmissionContractDetails);
                detail.ED1920Claimed1619 = GetClaimedValue(submission.SubmissionId, "1619ED1920", submission.SubmissionValues);

                submissionsDetails.Add(detail);
            }

            foreach (var provider in expectedProvidersNotSubmitted)
            {
                var detail = new FundingClaimsSubmissionsDetail
                {
                    UkPrn = provider.Ukprn,
                    ProviderName = orgDetails.GetValueOrDefault((int)provider.Ukprn)?.Name,
                    ExpectedToReturnInCurrentPeriod = "Yes",
                    ReturnedInCurrentPeriod = "No"
                };

                submissionsDetails.Add(detail);
            }

            model.FundingClaimsSubmissionsDetails = submissionsDetails;
            return model;
        }

        private decimal GetClaimedValue(Guid submissionId, string fundingStreamPeriodCode, ICollection<FundingClaimSubmissionsValue> submissionSubmissionValues)
        {
            decimal result = 0;
            var fundingClaimSubmissionsValues = submissionSubmissionValues.Where(x => x.SubmissionId == submissionId && x.FundingStreamPeriodCode == fundingStreamPeriodCode).ToList();
            if (fundingClaimSubmissionsValues.Any())
            {
                result = fundingClaimSubmissionsValues.Sum(x => x.TotalDelivery);
            }

            return result;
        }

        public string isExpectedToReturn(long ukprn, List<OrganisationCollection> expectedProviders)
        {
            return expectedProviders.Any(x => x.Ukprn == ukprn) ? "Yes" : "No";
        }

        public decimal GetContractValue(Guid submissionId, string fundingStreamPeriodCode, ICollection<FundingClaimSubmissionContractDetail> contractDetails)
        {
            var value = contractDetails.FirstOrDefault(x => x.SubmissionId == submissionId && x.FundingStreamPeriodCode == fundingStreamPeriodCode);
            return value?.ContractValue ?? 0;
        }
    }
}
