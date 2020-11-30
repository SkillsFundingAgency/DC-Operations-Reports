using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Abstract;

namespace ESFA.DC.Operations.Reports.Reports.ILRProvidersReturningFirstTimePerDayReport
{
    public class ILRProvidersReturningFirstTimePerDayReportModelBuilder : AbstractILRSubmissionsModelBuilder<ILRProvidersReturningFirstTimePerDayModel>, IModelBuilder<ILRProvidersReturningFirstTimePerDayModel>
    {
        public ILRProvidersReturningFirstTimePerDayReportModelBuilder(IILRProvidersReturningFirstTimePerDayProviderService ilrFileSubmissionsPerDayService, IDateTimeProvider dateTimeProvider)
            : base(ilrFileSubmissionsPerDayService, dateTimeProvider)
        {
        }

        public override string Title => "ILR Providers Returning for the First Time Per Period";
    }
}
