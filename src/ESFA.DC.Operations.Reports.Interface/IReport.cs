using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.Operations.Reports.Interface
{
    public interface IReport
    {
        string TaskName { get; }

        Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken);
    }
}
