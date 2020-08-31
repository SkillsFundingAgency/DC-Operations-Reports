using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Reports.ILRFileSubmissionPerDayReport
{
    public class ILRFileSubmissionPerDayReportModelBuilder : IModelBuilder<ILRFileSubmissionsPerDayModel>
    {
        private readonly IILRFileSubmissionsPerDayProviderService _ilrFileSubmissionsPerDayProviderService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly string Title = "ILR File Submissions per Day per Period";
        private readonly string ChartScale = "(Log 2 Scale)";

        public ILRFileSubmissionPerDayReportModelBuilder(
            IILRFileSubmissionsPerDayProviderService ilrFileSubmissionsPerDayProviderService,
            IDateTimeProvider dateTimeProvider)
        {
            _ilrFileSubmissionsPerDayProviderService = ilrFileSubmissionsPerDayProviderService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ILRFileSubmissionsPerDayModel> Build(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var model = new ILRFileSubmissionsPerDayModel();

            var ilrProvidersReturningFirstTimePerDay = await _ilrFileSubmissionsPerDayProviderService.GetILRFileSubmissionsPerDay(reportServiceContext.CollectionYear, reportServiceContext.Period, cancellationToken);

            model.IlrFileSubmissionsPerDayList = ilrProvidersReturningFirstTimePerDay.ToList();
            model.Period = reportServiceContext.ReturnPeriodName;
            model.ReportTitle = $"{reportServiceContext.CollectionYear} {Title} - {_dateTimeProvider.ConvertUtcToUk(reportServiceContext.SubmissionDateTimeUtc):dd MMM yyyy HH:mm:ss}";
            model.ChartTitle =  $"{reportServiceContext.CollectionYear} {Title} {ChartScale} - {model.Period}";
            return model;
        }
    }
}
