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

        public async Task<CollectionDetail> GetLatestCollectionDetailAsync(int collectionYear, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"return GetLatestCollectionDetailAsync : {collectionYear}");

                using (IFundingClaimsDataContext context = _fundingClaimsContextFactory())
                {
                    return await context.CollectionDetail.Where(x => x.CollectionYear == collectionYear)
                        .OrderByDescending(x => x.SubmissionCloseDateUtc)
                        .Select(x => new CollectionDetail()
                        {
                            CollectionId = x.CollectionId,
                            CollectionName = x.CollectionName,
                            DisplayTitle = x.DisplayTitle,
                            SubmissionOpenDateUtc = x.SubmissionOpenDateUtc,
                            SubmissionCloseDateUtc = x.SubmissionCloseDateUtc
                        })
                        .FirstOrDefaultAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"error GetLatestCollectionDetailAsync : {collectionYear}", e);
                throw;
            }
        }

        public async Task<IEnumerable<FundingClaimsSubmission>> GetAllFundingClaimsSubmissionsByCollectionAsync(int collectionId, CancellationToken cancellationToken)
        {
            var items = new List<FundingClaimsSubmission>();

            try
            {
                using (IFundingClaimsDataContext context = _fundingClaimsContextFactory())
                {

                   items = await (from s in context.Submission
                                     let version = (from d in context.Submission
                                                    where d.CollectionId == collectionId && s.Ukprn == d.Ukprn
                            select d.Version).Max()
                        where s.Version == version && s.CollectionId == collectionId
                        select new FundingClaimsSubmission
                        {
                            SubmissionId = s.SubmissionId,
                            Ukprn = s.Ukprn,
                            SubmittedDateTimeUtc = s.SubmittedDateTimeUtc,
                            Version = s.Version,
                            CollectionId = s.CollectionId,
                            IsSubmitted = s.IsSubmitted,
                            SubmissionValues = s.SubmissionValue.Select(sv => new FundingClaimSubmissionsValue()
                            {
                                SubmissionId = sv.SubmissionId,
                                TotalDelivery = sv.TotalDelivery,
                                ContractAllocationNumber = sv.ContractAllocationNumber,
                                FundingStreamPeriodCode = sv.FundingStreamPeriodCode,
                            }).ToList(),
                            SubmissionContractDetails = s.SubmissionContractDetail.Select(sc => new FundingClaimSubmissionContractDetail()
                            {
                                SubmissionId = sc.SubmissionId,
                                FundingStreamPeriodCode = sc.FundingStreamPeriodCode,
                                ContractValue = sc.ContractValue,
                            }).ToList(),
                        }).ToListAsync(cancellationToken);


                    //var items1 = await context.Submission
                    //    .Where(x => x.CollectionId == collectionId && 
                    //                x.Version == context.Submission.Where(s => s.Ukprn == x.Ukprn 
                    //                                                           && x.CollectionId == collectionId).Max(s => s.Version))
                    //    .Select(x => new FundingClaimsSubmission()
                    //     {
                    //         SubmissionId = x.SubmissionId,
                    //         Ukprn = x.Ukprn,
                    //         SubmittedDateTimeUtc = x.SubmittedDateTimeUtc.GetValueOrDefault(),
                    //         Version = x.Version,
                    //         CollectionId = x.CollectionId,
                    //         IsSubmitted = x.IsSubmitted,
                    //         SubmissionValues = x.SubmissionValue.Select( sv => new FundingClaimSubmissionsValue(){
                    //             SubmissionId = sv.SubmissionId,
                    //             TotalDelivery = sv.TotalDelivery,
                    //             ContractAllocationNumber =  sv.ContractAllocationNumber,
                    //             FundingStreamPeriodCode = sv.FundingStreamPeriodCode
                    //                        }).ToList(),
                    //         SubmissionContractDetails = x.SubmissionContractDetail.Select(sc => new FundingClaimSubmissionContractDetail()
                    //         {
                    //             SubmissionId = sc.SubmissionId,
                    //             FundingStreamPeriodCode = sc.FundingStreamPeriodCode,
                    //             ContractValue = sc.ContractValue
                    //         }).ToList(),
                    //     }).ToListAsync(cancellationToken);
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
