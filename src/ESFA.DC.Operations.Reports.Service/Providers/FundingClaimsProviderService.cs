using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FundingClaims.Data;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model.FundingClaims;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.Operations.Reports.Service.Providers
{
    public class FundingClaimsProviderService : IFundingClaimsProviderService
    {
        private readonly Func<IFundingClaimsDataContext> _fundingClaimsContextFactory;
        private readonly ILogger _logger;

        public FundingClaimsProviderService(
            Func<IFundingClaimsDataContext> fundingClaimsContextFactory,
            ILogger logger)
        {
            _fundingClaimsContextFactory = fundingClaimsContextFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<FundingClaimsSubmission>> GetAllFundingClaimsSubmissionsByCollectionAsync(int collectionId, CancellationToken cancellationToken)
        {
            var items = new List<FundingClaimsSubmission>();

            try
            {
                using (IFundingClaimsDataContext context = _fundingClaimsContextFactory())
                {
                   items = await context.Submission.Where(x => x.CollectionId == collectionId && x.IsSubmitted == true)
                        .Select(x => new FundingClaimsSubmission()
                        {
                           SubmissionId = x.SubmissionId,
                           Ukprn = x.Ukprn,
                           SubmittedDateTimeUtc = x.SubmittedDateTimeUtc.GetValueOrDefault(),
                           Version = x.Version,
                           CollectionId = x.CollectionId,
                           IsSubmitted = x.IsSubmitted
                        })
                        .ToListAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"error getting submissions for collectionId : {collectionId}", e);
                throw;
            }

            _logger.LogInfo($"return submissions for collectionId : {collectionId}");

            return items;
        }
    }
}
