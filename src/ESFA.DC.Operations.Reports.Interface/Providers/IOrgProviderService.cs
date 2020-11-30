using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Interface.Providers
{
    public interface IOrgProviderService
    {
        Task<IDictionary<int, OrgModel>> GetOrgDetailsForUKPRNsAsync(List<long> uKPRNs, CancellationToken cancellationToken);

        bool IsValidUpin(string upin);
    }
}
