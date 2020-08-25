using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Reports.ILRProvidersReturningFirstTimePerDayReport
{
    public class ILRProvidersReturningFirstTimePerDayReportModelBuilder : IModelBuilder<ILRProvidersReturningFirstTimePerDayModel>
    {
        private readonly IILRProvidersReturningFirstTimePerDayProviderService _ilrProvidersReturningFirstTimePerDayProviderService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly string Title = "ILR Providers Returning for the First Time Per Period";
        private readonly string ChartScale = "(Log 2 Scale)";

        public ILRProvidersReturningFirstTimePerDayReportModelBuilder(
            IILRProvidersReturningFirstTimePerDayProviderService ilrProvidersReturningFirstTimePerDayProviderService,
            IDateTimeProvider dateTimeProvider)
        {
            _ilrProvidersReturningFirstTimePerDayProviderService = ilrProvidersReturningFirstTimePerDayProviderService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ILRProvidersReturningFirstTimePerDayModel> Build(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var model = new ILRProvidersReturningFirstTimePerDayModel();

            var ilrProvidersReturningFirstTimePerDay = await _ilrProvidersReturningFirstTimePerDayProviderService.GetILRProvidersReturningFirstTimePerDay(reportServiceContext.CollectionYear, reportServiceContext.Period, cancellationToken);

            model.IlrProvidersReturningFirstTimePerDaysList = ilrProvidersReturningFirstTimePerDay.ToList();
            model.Period = reportServiceContext.ReturnPeriodName;
            model.ReportTitle = $"{reportServiceContext.CollectionYear} {Title} - {_dateTimeProvider.ConvertUtcToUk(reportServiceContext.SubmissionDateTimeUtc):dd MMM yyyy HH:mm:ss}";
            model.ChartTitle =  $"{reportServiceContext.CollectionYear} {Title} {ChartScale} - {model.Period}";
            return model;
        }
    }
}
