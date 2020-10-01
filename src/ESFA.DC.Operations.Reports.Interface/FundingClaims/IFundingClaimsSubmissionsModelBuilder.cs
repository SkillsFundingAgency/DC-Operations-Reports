﻿using System;
using System.Collections.Generic;
using System.Threading;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;

namespace ESFA.DC.Operations.Reports.Interface.FundingClaims
{
    public interface IFundingClaimsSubmissionsModelBuilder
    {
        FundingClaimsSubmissionsModel Build(
                                            CollectionDetail collectionDetail,
                                            List<OrganisationCollection> expectedProviders,
                                            List<FundingClaimsSubmission> fundingClaimsSubmissions,
                                            IDictionary<int, OrgModel> orgDetails,
                                            CancellationToken cancellationToken);

        decimal GetClaimedValue(Guid submissionId, string fundingStreamPeriodCode, ICollection<FundingClaimSubmissionsValue> submissionSubmissionValues);

        decimal GetContractValue(Guid submissionId, string fundingStreamPeriodCode, ICollection<FundingClaimSubmissionContractDetail> contractDetails);

        string IsExpectedToReturn(long ukprn, List<OrganisationCollection> expectedProviders);
    }
}
