using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;

namespace ESFA.DC.Operations.Reports.Interface
{
    public interface IModelBuilder<T>
    {
        Task<T> Build(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken);
    }
}
