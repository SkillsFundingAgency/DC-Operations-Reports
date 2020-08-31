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
    public class ILRFileSubmissionsPerDayProviderService : IILRFileSubmissionsPerDayProviderService
    {
        private readonly IReportServiceConfiguration _reportServiceConfiguration;

        public ILRFileSubmissionsPerDayProviderService(
            ILogger logger,
            IReportServiceConfiguration reportServiceConfiguration)
        {
            _reportServiceConfiguration = reportServiceConfiguration;
        }

        public async Task<IEnumerable<ILRFileSubmissionsPerDay>> GetILRFileSubmissionsPerDay(int collectionYear, int period, CancellationToken cancellationToken)
        {
            var ilrFileSubmissionsPerDays = new List<ILRFileSubmissionsPerDay>();

            using (var connection = new SqlConnection(_reportServiceConfiguration.JobManagementConnectionString))
            {
                await connection.OpenAsync();

                ilrFileSubmissionsPerDays = (await connection.QueryAsync<ILRFileSubmissionsPerDay>("dbo.GetILRSubmissionsPerDay", new
                {
                    periodNumber = period,
                    collectionYear = collectionYear
                }, commandType: CommandType.StoredProcedure)).ToList();
            }

            return ilrFileSubmissionsPerDays;
        }
    }
}
