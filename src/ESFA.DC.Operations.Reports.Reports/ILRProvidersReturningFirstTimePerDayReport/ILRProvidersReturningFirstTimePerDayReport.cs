using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FileService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Abstract;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.Operations.Reports.Reports.ILRProvidersReturningFirstTimePerDayReport
{
    public class ILRProvidersReturningFirstTimePerDayReport : AbstractReport, IReport
    {
        private readonly IFileService _fileService;
        private readonly IJsonSerializationService _serializationService;
        private readonly IExcelFileService _excelFileService;
        private readonly IFileNameService _fileNameService;
        private readonly IModelBuilder<ILRProvidersReturningFirstTimePerDayModel> _modelBuilder;
        private const string TemplateName = "ILRProvidersReturningFirstTimePerDayTemplate.xlsx";
        private const string ReportDataSource = "IlrReturningProvidersInfo";

        public ILRProvidersReturningFirstTimePerDayReport(
            IFileService fileService,
            IJsonSerializationService serializationService,
            IExcelFileService excelFileService,
            IFileNameService fileNameService,
            IModelBuilder<ILRProvidersReturningFirstTimePerDayModel> modelBuilder)
            : base(ReportTaskNameConstants.ILRProvidersReturningFirstTimePerDayReport, "ILR Providers Returning First Time Per Day Report")
        {
            _fileService = fileService;
            _serializationService = serializationService;
            _excelFileService = excelFileService;
            _fileNameService = fileNameService;
            _modelBuilder = modelBuilder;
        }

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var ilrProvidersReturningFirstTimePerDayModel = await _modelBuilder.Build(reportServiceContext, cancellationToken);
            var reportFileName = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Excel, true, true, false);

            await GenerateWorkBookAsync(ilrProvidersReturningFirstTimePerDayModel, TemplateName, ReportDataSource, reportServiceContext, reportFileName, cancellationToken);
            return new[] { reportFileName };
        }

        private async Task GenerateWorkBookAsync(
             ILRProvidersReturningFirstTimePerDayModel model,
             string templateFileName,
             string dataSource,
             IOperationsReportServiceContext reportServiceContext,
             string reportFileName,
             CancellationToken cancellationToken)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(templateFileName));

            using (Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                var workbook = _excelFileService.BindExcelTemplateToWorkbook(model, dataSource, manifestResourceStream);
                await _excelFileService.SaveWorkbookAsync(workbook, reportFileName, reportServiceContext.Container, cancellationToken);
            }
        }
    }
}
