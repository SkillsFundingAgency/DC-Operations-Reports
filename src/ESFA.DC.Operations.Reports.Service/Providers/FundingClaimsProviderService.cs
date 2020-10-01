﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.FundingClaims.Data;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model.FundingClaims;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.Operations.Reports.Service.Providers
{
    public class FundingClaimsProviderService : IFundingClaimsProviderService
    {
        private readonly Func<IFundingClaimsDataContext> _fundingClaimsContextFactory;
        private readonly IReportServiceConfiguration _reportServiceConfiguration;
        private readonly ILogger _logger;

        private readonly string fundingClaimsSubmissionsSql = @"SELECT 
                                                                        a.SubmissionId,
                                                                        a.Ukprn,
                                                                        a.SubmittedDateTimeUtc,
                                                                        a.Version,
                                                                        a.CollectionId,
                                                                        a.IsSubmitted,
                                                                        a.CovidDeclaration,
                                                                        sv.Id as SubmissionValueId,
                                                                        sv.SubmissionId AS SubmissionValueSubmissionId,
                                                                        sv.TotalDelivery,
                                                                        sv.ContractAllocationNumber,
                                                                        sv.FundingStreamPeriodCode AS SubmissionValueFundingStreamPeriodCode,
                                                                        scd.Id as SubmissionContractDetailId,
                                                                        scd.SubmissionId AS SubmissionContractSubmissionId ,
                                                                        scd.FundingStreamPeriodCode AS SubmissionContractFundingStreamPeriodCode,
                                                                        scd.ContractValue
                                                                    FROM submission a 
                                                                        LEFT OUTER JOIN submissionvalue sv ON sv.submissionid  = a.SubmissionId
                                                                        LEFT OUTER JOIN SubmissionContractDetail scd ON scd.SubmissionId = a.SubmissionId
                                                                    WHERE CollectionId = @collectionId
                                                                                AND a.version = (SELECT MAX(b.version) FROM Submission b WHERE a.UKPRN = b.UKPRN AND collectionId = @collectionId )";

        public FundingClaimsProviderService(
            Func<IFundingClaimsDataContext> fundingClaimsContextFactory,
            IReportServiceConfiguration reportServiceConfiguration,
            ILogger logger)
        {
            _fundingClaimsContextFactory = fundingClaimsContextFactory;
            _reportServiceConfiguration = reportServiceConfiguration;
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
            var model = new List<FundingClaimsSubmission>();
            IEnumerable<FundingClaimsSubmissionResultSet> result = new List<FundingClaimsSubmissionResultSet>();
            try
            {
                using (var connection = new SqlConnection(_reportServiceConfiguration.FundingClaimsConnectionString))
                {
                    await connection.OpenAsync();
                    result = await connection.QueryAsync<FundingClaimsSubmissionResultSet>(fundingClaimsSubmissionsSql, new { collectionId });
                }

                var fundingClaimsSubmissions = result
                    .GroupBy(r => new { r.Version, r.SubmissionId, r.Ukprn, r.CollectionId, r.IsSubmitted, r.SubmittedDateTimeUtc, r.CovidDeclaration })
                    .Select(x => new FundingClaimsSubmission()
                    {
                        Version = x.Key.Version,
                        SubmissionId = x.Key.SubmissionId,
                        Ukprn = x.Key.Ukprn,
                        CollectionId = x.Key.CollectionId,
                        IsSubmitted = x.Key.IsSubmitted,
                        SubmittedDateTimeUtc = x.Key.SubmittedDateTimeUtc,
                        CovidDeclaration = x.Key.CovidDeclaration
                    }).Distinct().ToList();

                var fundingClaimsSubmissionValues = result
                    .GroupBy(r => new { r.SubmissionValueId, r.SubmissionValueSubmissionId, r.SubmissionValueFundingStreamPeriodCode, r.TotalDelivery, r.ContractAllocationNumber })
                    .Select(x => new FundingClaimSubmissionsValue()
                    {
                        SubmissionId = x.Key.SubmissionValueSubmissionId,
                        FundingStreamPeriodCode = x.Key.SubmissionValueFundingStreamPeriodCode,
                        TotalDelivery = x.Key.TotalDelivery,
                        ContractAllocationNumber = x.Key.ContractAllocationNumber
                    }).Distinct().ToList();

                var fundingClaimsSubmissionContractDetails = result
                    .GroupBy(r => new { r.SubmissionContractDetailId, r.SubmissionContractSubmissionId, r.SubmissionContractFundingStreamPeriodCode, r.ContractValue })
                    .Select(x => new FundingClaimSubmissionContractDetail()
                    {
                        SubmissionId = x.Key.SubmissionContractSubmissionId,
                        FundingStreamPeriodCode = x.Key.SubmissionContractFundingStreamPeriodCode,
                        ContractValue = x.Key.ContractValue
                    }).Distinct().ToList();

                foreach (var submission in fundingClaimsSubmissions)
                {
                    submission.SubmissionValues = fundingClaimsSubmissionValues.Where(x => x.SubmissionId == submission.SubmissionId).ToList();
                    submission.SubmissionContractDetails = fundingClaimsSubmissionContractDetails.Where(x => x.SubmissionId == submission.SubmissionId).ToList();
                    model.Add(submission);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"error getting submissions for collectionId : {collectionId}", e);
                throw;
            }

            _logger.LogInfo($"return submissions for collectionId : {collectionId}");

            return model;
        }
    }
}
