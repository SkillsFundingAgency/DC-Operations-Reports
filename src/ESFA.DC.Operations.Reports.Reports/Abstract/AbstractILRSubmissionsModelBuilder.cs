using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Reports.Abstract
{
    public abstract class AbstractILRSubmissionsModelBuilder<TModel>
        where TModel : BaseSubmissionsModel, new()
    {
        private readonly IILRSubmissionsProviderService _ilrSubmissionsProviderService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private string ChartScale = "(Log 2 Scale)";

        public AbstractILRSubmissionsModelBuilder(IILRSubmissionsProviderService ilrSubmissionsProviderService, IDateTimeProvider dateTimeProvider)
        {
            _ilrSubmissionsProviderService = ilrSubmissionsProviderService;
            _dateTimeProvider = dateTimeProvider;
        }

        public virtual string Title { get; set; }

        public async Task<TModel> Build(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var model = new TModel();

            var ilrProvidersReturningFirstTimePerDay = await _ilrSubmissionsProviderService.GetSubmissionsPerDay(reportServiceContext.CollectionYear, reportServiceContext.Period, cancellationToken);

            model.SubmissionsPerDayList = ilrProvidersReturningFirstTimePerDay.ToList();
            model.Period = reportServiceContext.ReturnPeriodName;
            model.ReportTitle = $"{reportServiceContext.CollectionYear} {Title} - {_dateTimeProvider.ConvertUtcToUk(reportServiceContext.SubmissionDateTimeUtc):dd MMM yyyy HH:mm:ss}";
            model.ChartTitle = $"{reportServiceContext.CollectionYear} {Title} {ChartScale} - {model.Period}";
            return model;
        }
    }
}
