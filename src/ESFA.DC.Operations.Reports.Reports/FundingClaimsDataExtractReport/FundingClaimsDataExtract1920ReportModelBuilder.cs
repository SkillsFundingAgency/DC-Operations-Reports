using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface.FundingClaims;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;
using ESFA.DC.Operations.Reports.Reports.Extensions;

namespace ESFA.DC.Operations.Reports.Reports.FundingClaimsProviderSubmissionsReport
{
    public class FundingClaimsDataExtract1920ReportModelBuilder : IFundingClaimsDataExtractModelBuilder
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public FundingClaimsDataExtract1920ReportModelBuilder(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public FundingClaimsDataExtractReportModel Build(
            CollectionDetail collectionDetail,
            ICollection<FundingClaimsDataExtractResultSet> fundingClaimsDataExtractResultSets,
            IDictionary<int, OrgModel> orgDetails,
            CancellationToken cancellationToken)
        {
            var model = new FundingClaimsDataExtractReportModel();
            var fundingClaimsDataExtractDetails = fundingClaimsDataExtractResultSets.Select(
                    fundingClaim => new FundingClaimsDataExtractDetail()
                    {
                        SubmissionId = fundingClaim.SubmissionId,
                        CollectionPeriod = collectionDetail.CollectionCode,
                        Ukprn = fundingClaim.Ukprn,
                        ProviderName = orgDetails.GetValueOrDefault((int)fundingClaim.Ukprn)?.Name,
                        UpdatedOn = fundingClaim.UpdatedOn.GetValueOrDefault().LongDateStringFormat(),
                        Declaration = fundingClaim.Declaration,
                        CovidDeclaration = fundingClaim.CovidDeclaration,
                        FundingStreamPeriodCode = fundingClaim.SubmissionValueFundingStreamPeriodCode,
                        MaximumContractValue = fundingClaim.ContractValue,
                        DeliverableCode = fundingClaim.DeliverableCode,
                        DeliverableDescription = fundingClaim.Description,
                        StudentNumbers = fundingClaim.StudentNumbers,
                        DeliveryToDate = fundingClaim.DeliveryToDate,
                        ForecastedDelivery = fundingClaim.ForecastedDelivery,
                        ExceptionalAdjustments = fundingClaim.ExceptionalAdjustments,
                        TotalDelivery = fundingClaim.TotalDelivery,
                        ContractAllocationNumber = fundingClaim.ContractAllocationNumber
                    })
                .OrderBy(x => x.ProviderName)
                .ThenBy(x => x.CollectionPeriod)
                .ThenBy(x => x.SubmissionId)
                .ThenBy(x => x.FundingStreamPeriodCode)
                .ThenBy(x => x.DeliverableCode)
                .ToList();

            model.FundingClaimsDataExtract = fundingClaimsDataExtractDetails;

            return model;
        }
    }
}
