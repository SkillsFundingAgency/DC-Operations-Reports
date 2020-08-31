using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Abstract;
using ESFA.DC.Operations.Reports.Reports.Constants;

namespace ESFA.DC.Operations.Reports.Reports.ILRFileSubmissionsPerDayReport
{
    public class ILRFileSubmissionPerDayReport : AbstractReport, IReport
    {
        private readonly IExcelFileService _excelFileService;
        private readonly IFileNameService _fileNameService;
        private readonly IModelBuilder<ILRFileSubmissionsPerDayModel> _modelBuilder;
        private const string TemplateName = "ILRFileSubmissionPerDayReport.xlsx";
        private const string ReportDataSource = "IlrSubmissionsInfo";

        public ILRFileSubmissionPerDayReport(
            IExcelFileService excelFileService,
            IFileNameService fileNameService,
            IModelBuilder<ILRFileSubmissionsPerDayModel> modelBuilder)
            : base(ReportTaskNameConstants.ILRFileSubmissionsPerDayReport, "ILR File Submissions Per Day Report")
        {
            _excelFileService = excelFileService;
            _fileNameService = fileNameService;
            _modelBuilder = modelBuilder;
        }

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var ilrFileSubmissionsPerDayModel = await _modelBuilder.Build(reportServiceContext, cancellationToken);
            var reportFileName = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Excel, true, true, false);

            await GenerateWorkBookAsync(ilrFileSubmissionsPerDayModel, TemplateName, ReportDataSource, reportServiceContext, reportFileName, cancellationToken);
            return new[] { reportFileName };
        }

        private async Task GenerateWorkBookAsync(
             ILRFileSubmissionsPerDayModel model,
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
