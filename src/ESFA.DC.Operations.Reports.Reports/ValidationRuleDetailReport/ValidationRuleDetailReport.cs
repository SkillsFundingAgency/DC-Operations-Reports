using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CollectionsManagement.Models;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Reports.Constants;

namespace ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport
{
    public class ValidationRuleDetailReport : IReport
    {
        private readonly IValidationRuleDetailsProviderService _providerService;

        public ValidationRuleDetailReport(IValidationRuleDetailsProviderService providerService)
        {
            _providerService = providerService;
        }

        public string TaskName => ReportTaskNameConstants.ValidationRuleDetailReport;

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            //var ilrPeriodsAdjustedTimes = reportServiceContext.ILRPeriodsAdjustedTimes;

            var ilrPeriodsAdjustedTimes = BuildReturnPeriodsModel();

            var rule = reportServiceContext.Rule;

            var validationRuleDetails = _providerService.ProvideAsync(rule, ilrPeriodsAdjustedTimes, cancellationToken);
            return new[] { "success" };
        }

        private List<ReturnPeriod> BuildReturnPeriodsModel()
        {
            return new List<ReturnPeriod>
            {
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 08, 22, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2019, 09, 05, 15, 30, 45),
                    PeriodNumber = 1,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 09, 17, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2019, 10, 04, 15, 30, 45),
                    PeriodNumber = 2,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 10, 16, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2019, 11, 06, 15, 30, 45),
                    PeriodNumber = 3,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 11, 18, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2019, 12, 05, 15, 30, 45),
                    PeriodNumber = 4,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 12, 17, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 01, 07, 15, 30, 45),
                    PeriodNumber = 5,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 01, 17, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 02, 06, 15, 30, 45),
                    PeriodNumber = 6,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 02, 18, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 03, 05, 15, 30, 45),
                    PeriodNumber = 7,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 03, 17, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 04, 06, 15, 30, 45),
                    PeriodNumber = 8,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 04, 20, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 05, 06, 15, 30, 45),
                    PeriodNumber = 9,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 05, 19, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 06, 04, 15, 30, 45),
                    PeriodNumber = 10,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 06, 16, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 07, 06, 15, 30, 45),
                    PeriodNumber = 11,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 07, 16, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 08, 06, 15, 30, 45),
                    PeriodNumber = 12,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 08, 07, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 09, 13, 15, 30, 45),
                    PeriodNumber = 13,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 09, 14, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 10, 17, 15, 30, 45),
                    PeriodNumber = 14,
                },
            };
        }
    }
}
