using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ESFA.DC.CollectionsManagement.Models;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.FileService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Abstract;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport
{
    public class ValidationRuleDetailReport : AbstractReport, IReport
    {
        private readonly IIndex<ILRYears, IValidationRuleDetailsProviderService> _validationRulesProviderServices;
        private readonly IOrgProviderService _orgProviderService;
        private readonly IFileService _fileService;
        private readonly IJsonSerializationService _serializationService;
        private readonly ICsvFileService _csvFileService;
        private readonly IFileNameService _fileNameService;

        public ValidationRuleDetailReport(
            IIndex<ILRYears, IValidationRuleDetailsProviderService> validationRulesProviderServices,
            IOrgProviderService orgProviderService,
            IFileService fileService,
            IJsonSerializationService serializationService,
            ICsvFileService csvFileService,
            IFileNameService fileNameService)
            : base(ReportTaskNameConstants.ValidationRuleDetailReport, "Validation Rule Details")
        {
            _validationRulesProviderServices = validationRulesProviderServices;
            _orgProviderService = orgProviderService;
            _fileService = fileService;
            _serializationService = serializationService;
            _csvFileService = csvFileService;
            _fileNameService = fileNameService;
        }

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            //var ilrPeriodsAdjustedTimes = reportServiceContext.ILRPeriodsAdjustedTimes;

            var ilrPeriodsAdjustedTimes = BuildReturnPeriodsModel();
            var rule = reportServiceContext.Rule;
            var validationRuleDetailsProviderService = _validationRulesProviderServices[(ILRYears)reportServiceContext.Year];

            var validationRuleDetails = await validationRuleDetailsProviderService.GetValidationRuleDetails(rule, ilrPeriodsAdjustedTimes, cancellationToken);
            var ukprns = validationRuleDetails.Where(x => x.UkPrn != null).Select(x => (long)x.UkPrn);

            IEnumerable<OrgModel> orgDetails = await _orgProviderService.GetOrgDetailsForUKPRNsAsync(ukprns.Distinct().ToList(), CancellationToken.None);
            PopulateModelsWithOrgDetails(validationRuleDetails, orgDetails);

            var fileNameCsv = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Csv, true, false);
            var fileNameJson = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Json,true, false);

            using (var stream = await _fileService.OpenWriteStreamAsync(fileNameJson, reportServiceContext.Container, cancellationToken))
            {
                _serializationService.Serialize(validationRuleDetails, stream);
            }

            await _csvFileService.WriteAsync<ValidationRuleDetail, ValidationRuleDetailReportClassMap>(validationRuleDetails, fileNameCsv, reportServiceContext.Container, cancellationToken);
            return new[] { fileNameCsv };
        }

        public static void PopulateModelsWithOrgDetails(IEnumerable<ValidationRuleDetail> validationRuleDetails, IEnumerable<OrgModel> orgDetails)
        {
            foreach (var validationRuleDetail in validationRuleDetails)
            {
                validationRuleDetail.ProviderName = orgDetails.FirstOrDefault(p => p.Ukprn == validationRuleDetail.UkPrn).Name;
            }
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
