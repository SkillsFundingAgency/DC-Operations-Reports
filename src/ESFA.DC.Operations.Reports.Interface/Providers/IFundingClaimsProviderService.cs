using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;

namespace ESFA.DC.Operations.Reports.Interface.Providers
{
    public interface IFundingClaimsProviderService
    {
        Task<CollectionDetail> GetLatestCollectionDetailAsync(int collectionYear, CancellationToken cancellationToken);

        Task<ICollection<FundingClaimsSubmission>> GetAllFundingClaimsSubmissionsByCollectionAsync(int collectionId, CancellationToken cancellationToken);
    }
}
