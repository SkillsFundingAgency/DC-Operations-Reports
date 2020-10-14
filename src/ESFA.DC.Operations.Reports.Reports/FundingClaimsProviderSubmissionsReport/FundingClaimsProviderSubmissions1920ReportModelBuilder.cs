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

namespace ESFA.DC.Operations.Reports.Reports.FundingClaimsProviderSubmissionsReport
{
    public class FundingClaimsProviderSubmissions1920ReportModelBuilder : IFundingClaimsSubmissionsModelBuilder
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public FundingClaimsProviderSubmissions1920ReportModelBuilder(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public FundingClaimsSubmissionsModel Build(
            CollectionDetail collectionDetail,
            ICollection<OrganisationCollection> expectedProviders,
            ICollection<FundingClaimsSubmission> fundingClaimsSubmissions,
            IDictionary<int, OrgModel> orgDetails,
            CancellationToken cancellationToken)
        {
            var submittedProviderUkprns = fundingClaimsSubmissions.Select(x => x.Ukprn).ToList();
            var expectedUkprns = expectedProviders.Select(x => (long)x.Ukprn).ToList();

            var expectedProvidersNotSubmitted = expectedProviders.Where(x => !submittedProviderUkprns.Contains(x.Ukprn)).ToList();
            var expectedProvidersSubmitted = expectedProviders.Where(x => submittedProviderUkprns.Contains(x.Ukprn)).ToList();
            var unexpectedReturningProviders = submittedProviderUkprns.Where(x => !expectedUkprns.Contains(x)).ToList();
            var totalProviders = expectedUkprns.Union(submittedProviderUkprns).Count();

            var model = new FundingClaimsSubmissionsModel
            {
                FundingClaim = collectionDetail.DisplayTitle,
                ReportRun = _dateTimeProvider.ConvertUtcToUk(_dateTimeProvider.GetNowUtc()).LongDateStringFormat(),
                TotalNoOfProviders = totalProviders,
                NoOfProvidersExpectedToReturn = expectedUkprns.Count,
                NoOfReturningExpectedProviders = expectedProvidersSubmitted.Count,
                NoOfExpectedProvidersNotReturning = expectedProvidersNotSubmitted.Count,
                NoOfReturningUnexpectedProviders = unexpectedReturningProviders.Count
            };

            var submissionsDetails = new List<FundingClaimsSubmissionsDetail>();
            foreach (var submission in fundingClaimsSubmissions)
            {
                var detail = new FundingClaimsSubmissionsDetail
                {
                    UkPrn = submission.Ukprn,
                    ProviderName = orgDetails.GetValueOrDefault((int)submission.Ukprn)?.Name,
                    ExpectedToReturnInCurrentPeriod = IsExpectedToReturn(submission.Ukprn, expectedProviders),
                    ReturnedInCurrentPeriod = submission.IsSubmitted ? "Yes" : "No",
                    DateLatestClaimSubmitted = submission.SubmittedDateTimeUtc?.LongDateStringFormat(),
                    CovidResponse = BuildCovidResponse(submission.CovidDeclaration),
                    Signed = BuildSignedResponse(submission.IsSigned, submission.IsSubmitted)
                };

                detail.ALLBC1920ContractValue = GetContractValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.ALLBC1920, submission.SubmissionContractDetails);
                detail.ALLBC1920Claimed = GetClaimedValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.ALLBC1920, submission.SubmissionValues);

                detail.AEBCASCL1920ContractValue = GetContractValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.AEBCASCL1920, submission.SubmissionContractDetails);
                detail.AEBCASCL1920Claimed = GetClaimedValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.AEBCASCL1920, submission.SubmissionValues);

                detail.AEBC19TRN1920ContractValue = GetContractValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.AEBC19TRN1920, submission.SubmissionContractDetails);
                detail.AEBC19TRN1920Claimed = GetClaimedValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.AEBC19TRN1920, submission.SubmissionValues);

                detail.AEBASLS1920ProcuredContractValue = GetContractValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.AEBASLS1920, submission.SubmissionContractDetails);
                detail.AEBASLS1920ProcuredClaimed = GetClaimedValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.AEBASLS1920, submission.SubmissionValues);

                detail.AEB19TRLS1920ProcuredContractValue = GetContractValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.AEB19TRLS1920, submission.SubmissionContractDetails);
                detail.AEB19TRLS1920ProcuredClaimed = GetClaimedValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.AEB19TRLS1920, submission.SubmissionValues);

                detail.ED1920ContractValue1619 = GetContractValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.C1619ED1920, submission.SubmissionContractDetails);
                detail.ED1920Claimed1619 = GetClaimedValue(submission.SubmissionId, FundingStreamPeriodCodeConstants.C1619ED1920, submission.SubmissionValues);

                submissionsDetails.Add(detail);
            }

            foreach (var provider in expectedProvidersNotSubmitted)
            {
                var detail = new FundingClaimsSubmissionsDetail
                {
                    UkPrn = provider.Ukprn,
                    ProviderName = orgDetails.GetValueOrDefault(provider.Ukprn)?.Name,
                    ExpectedToReturnInCurrentPeriod = "Yes",
                    ReturnedInCurrentPeriod = "No",
                    CovidResponse = "N/A",
                };

                submissionsDetails.Add(detail);
            }

            model.FundingClaimsSubmissionsDetails = submissionsDetails.OrderBy(x => x.ProviderName).ToList();
            return model;
        }

        public string BuildSignedResponse(bool isSigned, bool isSubmitted)
        {
            if (isSubmitted)
            {
                return isSigned ? "Yes" : "No";
            }

            return string.Empty;
        }

        public string BuildCovidResponse(bool? covidDeclaration)
        {
            if (covidDeclaration == null)
            {
                return "N/A";
            }

            return covidDeclaration.GetValueOrDefault() ? "Yes" : "No";
        }

        public decimal GetClaimedValue(Guid submissionId, string fundingStreamPeriodCode, ICollection<FundingClaimSubmissionsValue> submissionSubmissionValues)
        {
            decimal result = 0;
            var fundingClaimSubmissionsValues = submissionSubmissionValues.Where(x => x.SubmissionId == submissionId && x.FundingStreamPeriodCode == fundingStreamPeriodCode).ToList();
            if (fundingClaimSubmissionsValues.Any())
            {
                result = fundingClaimSubmissionsValues.Sum(x => x.TotalDelivery);
            }

            return result;
        }

        public string IsExpectedToReturn(long ukprn, ICollection<OrganisationCollection> expectedProviders)
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
