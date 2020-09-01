using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Service.Providers.Abstract;

namespace ESFA.DC.Operations.Reports.Service.Providers
{
    public class ILRFileSubmissionsPerDayProviderService : AbstractILRSubmissionsPerDayService, IILRFileSubmissionsPerDayProviderService
    {
        public ILRFileSubmissionsPerDayProviderService(
            ILogger logger,
            IReportServiceConfiguration reportServiceConfiguration)
            : base(reportServiceConfiguration)
        {
        }

        public override string StoredProcedure => "dbo.GetILRSubmissionsPerDay";
    }
}
