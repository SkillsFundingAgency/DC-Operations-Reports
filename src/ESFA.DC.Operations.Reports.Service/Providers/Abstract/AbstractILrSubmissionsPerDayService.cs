using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Service.Providers.Abstract
{
    public abstract class AbstractILRSubmissionsPerDayService
    {
        private readonly IReportServiceConfiguration _reportServiceConfiguration;

        public AbstractILRSubmissionsPerDayService(IReportServiceConfiguration reportServiceConfiguration)
        {
            _reportServiceConfiguration = reportServiceConfiguration;
        }

        public virtual string StoredProcedure { get; set; }

        public async Task<IEnumerable<SubmissionsPerDay>> GetSubmissionsPerDay(int collectionYear, int period, CancellationToken cancellationToken)
        {
            var submissionsPerDay = new List<SubmissionsPerDay>();

            using (var connection = new SqlConnection(_reportServiceConfiguration.JobManagementConnectionString))
            {
                await connection.OpenAsync();

                submissionsPerDay = (await connection.QueryAsync<SubmissionsPerDay>(
                    StoredProcedure,
                    new
                {
                    periodNumber = period,
                    collectionYear = collectionYear
                }, commandType: CommandType.StoredProcedure)).ToList();
            }

            return submissionsPerDay;
        }
    }
}
