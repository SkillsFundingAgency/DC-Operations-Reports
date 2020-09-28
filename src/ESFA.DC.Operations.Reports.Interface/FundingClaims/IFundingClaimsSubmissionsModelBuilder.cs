using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;

namespace ESFA.DC.Operations.Reports.Interface.FundingClaims
{
    public interface IFundingClaimsSubmissionsModelBuilder
    {
        Task<FundingClaimsSubmissionsModel> Build(CollectionDetail collectionDetail,
            IEnumerable<OrganisationCollection> expectedProviders,
            IEnumerable<FundingClaimsSubmission> fundingClaimsSubmissions, IDictionary<int, OrgModel> orgDetails,
            CancellationToken cancellationToken);
    }
}
