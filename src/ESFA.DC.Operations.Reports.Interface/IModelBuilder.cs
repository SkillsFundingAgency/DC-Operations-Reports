using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.Operations.Reports.Interface
{
    public interface IModelBuilder<T>
    {
        Task<T> Build(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken);
    }
}
