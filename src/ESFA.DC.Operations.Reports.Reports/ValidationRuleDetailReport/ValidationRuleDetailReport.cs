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
        private readonly IFileService _fileService;
        private readonly IJsonSerializationService _serializationService;
        private readonly ICsvFileService _csvFileService;
        private readonly IFileNameService _fileNameService;
        private readonly IModelBuilder<IEnumerable<ValidationRuleDetail>> _modelBuilder;

        public ValidationRuleDetailReport(
            IFileService fileService,
            IJsonSerializationService serializationService,
            ICsvFileService csvFileService,
            IFileNameService fileNameService,
            IModelBuilder<IEnumerable<ValidationRuleDetail>> modelBuilder)
            : base(ReportTaskNameConstants.ValidationRuleDetailReport, "Validation Rule Details")
        {
            _fileService = fileService;
            _serializationService = serializationService;
            _csvFileService = csvFileService;
            _fileNameService = fileNameService;
            _modelBuilder = modelBuilder;
        }

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var validationRuleDetails = await _modelBuilder.Build(reportServiceContext, cancellationToken);
            var fileNameCsv = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Csv, true, false);
            var fileNameJson = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Json,true, false);

            var validationRuleDetailsList = validationRuleDetails.ToList();
            using (var stream = await _fileService.OpenWriteStreamAsync(fileNameJson, reportServiceContext.Container, cancellationToken))
            {
                _serializationService.Serialize(validationRuleDetailsList, stream);
            }

            await _csvFileService.WriteAsync<ValidationRuleDetail, ValidationRuleDetailReportClassMap>(validationRuleDetailsList, fileNameCsv, reportServiceContext.Container, cancellationToken);
            return new[] { fileNameCsv };
        }
    }
}
