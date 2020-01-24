using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Reports.Constants;

namespace ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport
{
    public class ValidationRuleDetailReport: IReport
    {
        public string TaskName => ReportTaskNameConstants.ValidationRuleDetailReport;

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            return new[] { "success" };
        }
    }
}
