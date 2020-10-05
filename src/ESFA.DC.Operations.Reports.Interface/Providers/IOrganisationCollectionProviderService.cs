using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Interface.Providers
{
    public interface IOrganisationCollectionProviderService
    {
        Task<ICollection<OrganisationCollection>> GetOrganisationCollectionsByCollectionIdAsync(int collectionId, CancellationToken cancellationToken);
    }
}
