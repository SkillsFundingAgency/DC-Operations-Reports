using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.PIMS.EF.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.Operations.Reports.Service.Providers
{
    public class OrgProviderService : IOrgProviderService
    {
        private readonly Func<IPimsContext> _orgContextFactory;

        public OrgProviderService(Func<IPimsContext> orgContextFactory)
        {
            _orgContextFactory = orgContextFactory;
        }

        public async Task<IDictionary<int, OrgModel>> GetOrgDetailsForUKPRNsAsync(List<long> uKPRNs, CancellationToken cancellationToken)
        {
            List<OrgModel> orgModels = new List<OrgModel>();

            if ((uKPRNs?.Count ?? 0) == 0)
            {
                return new Dictionary<int, OrgModel>();
            }

            int count = uKPRNs.Count;
            int pageSize = 1000;

            using (var orgContext = _orgContextFactory())
            {
                for (int i = 0; i < count; i += pageSize)
                {
                    List<OrgModel> orgs = await orgContext.Orgs
                        .Where(x => uKPRNs.Skip(i).Take(pageSize).Contains((long)x.OrgUkprn.Ukprn) && x.StatusId == 1)
                        .Select(x => new OrgModel { Ukprn = (long)x.OrgUkprn.Ukprn, Name = x.OrgName })
                        .ToListAsync(cancellationToken);

                    orgModels.AddRange(orgs);
                }
            }

            return orgModels.ToDictionary(k => (int) k.Ukprn, v => v);
        }
    }
}
