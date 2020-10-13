using System;
using System.Collections.Generic;
using System.Threading;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;

namespace ESFA.DC.Operations.Reports.Interface.FundingClaims
{
    public interface IFundingClaimsDataExtractModelBuilder
    {
        FundingClaimsDataExtractReportModel Build(
                                            CollectionDetail collectionDetail,
                                            ICollection<FundingClaimsDataExtractResultSet> fundingClaimsDataExtractResultSets,
                                            IDictionary<int, OrgModel> orgDetails,
                                            CancellationToken cancellationToken);
    }
}
