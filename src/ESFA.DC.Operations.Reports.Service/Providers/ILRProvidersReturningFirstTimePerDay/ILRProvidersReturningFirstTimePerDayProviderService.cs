using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Service.Providers
{
    public class ILRProvidersReturningFirstTimePerDayProviderService : IILRProvidersReturningFirstTimePerDayProviderService
    {
        private readonly IReportServiceConfiguration _reportServiceConfiguration;

        public ILRProvidersReturningFirstTimePerDayProviderService(
            ILogger logger,
            IReportServiceConfiguration reportServiceConfiguration)
        {
            _reportServiceConfiguration = reportServiceConfiguration;
        }

        public async Task<IEnumerable<ILRProvidersReturningFirstTimePerDay>> GetILRProvidersReturningFirstTimePerDay(int collectionYear, int period, CancellationToken cancellationToken)
        {
            var ilrProvidersReturningFirstTimePerDays = new List<ILRProvidersReturningFirstTimePerDay>();

            using (var connection = new SqlConnection(_reportServiceConfiguration.JobManagementConnectionString))
            {
                await connection.OpenAsync();

                ilrProvidersReturningFirstTimePerDays = (await connection.QueryAsync<ILRProvidersReturningFirstTimePerDay>( "dbo.GetIlrProvidersReturningFirstTimePerDay", new
                {
                    periodNumber = period,
                    collectionYear = collectionYear
                }, commandType : CommandType.StoredProcedure)).ToList();
            }

            return ilrProvidersReturningFirstTimePerDays;
        }
    }
}
