using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Cells;
using Autofac.Features.Indexed;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Operations.Reports.Reports.Extensions;

namespace ESFA.DC.Operations.Reports.Reports.ILRProvidersReturningFirstTimePerDayReport
{
    public class ILRProvidersReturningFirstTimePerDayReportModelBuilder : IModelBuilder<ILRProvidersReturningFirstTimePerDayModel>
    {
        private readonly IILRProvidersReturningFirstTimePerDayProviderService _ilrProvidersReturningFirstTimePerDayProviderService;

        public ILRProvidersReturningFirstTimePerDayReportModelBuilder(
            IILRProvidersReturningFirstTimePerDayProviderService ilrProvidersReturningFirstTimePerDayProviderServices)
        {
            _ilrProvidersReturningFirstTimePerDayProviderService = ilrProvidersReturningFirstTimePerDayProviderServices;
        }

        public async Task<ILRProvidersReturningFirstTimePerDayModel> Build(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var ilrProvidersReturningFirstTimePerDayModel = await _ilrProvidersReturningFirstTimePerDayProviderService.GetILRProvidersReturningFirstTimePerDay(cancellationToken);

            return ilrProvidersReturningFirstTimePerDayModel;
        }
    }
}
