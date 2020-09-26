using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Service.Providers
{
    public class OrganisationCollectionProviderService : IOrganisationCollectionProviderService
    {
        private readonly IReportServiceConfiguration _reportServiceConfiguration;

        private readonly string organisationCollectionSql = @"SELECT 
	                                                               OrganisationId
                                                                  ,CollectionId
                                                                  ,Ukprn
                                                              FROM [dbo].[OrganisationCollection]
                                                              WHERE collectionid = @collectionId";

        public OrganisationCollectionProviderService(IReportServiceConfiguration reportServiceConfiguration)
        {
            _reportServiceConfiguration = reportServiceConfiguration;
        }

        public async Task<IEnumerable<OrganisationCollection>> GetOrganisationCollectionsByCollectionIdAsync(int collectionId, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_reportServiceConfiguration.JobManagementConnectionString))
            {
                await connection.OpenAsync();

                var result = await connection.QueryAsync<OrganisationCollection>(organisationCollectionSql, new { collectionId });

                return result.ToList();
            }
        }
    }
}
