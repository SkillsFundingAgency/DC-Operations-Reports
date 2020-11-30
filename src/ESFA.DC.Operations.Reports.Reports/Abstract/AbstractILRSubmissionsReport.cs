using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Reports.Abstract
{
    public abstract class AbstractILRSubmissionsReport<TModel> : AbstractReport, IReport
    {
        private readonly IModelBuilder<TModel> _modelBuilder;
        private readonly IExcelFileService _excelFileService;
        private readonly IFileNameService _fileNameService;

        protected AbstractILRSubmissionsReport(
            string taskName,
            string fileName,
            IModelBuilder<TModel> modelBuilder,
            IExcelFileService excelFileService,
            IFileNameService fileNameService) : base(taskName, fileName)
        {
            _modelBuilder = modelBuilder;
            _excelFileService = excelFileService;
            _fileNameService = fileNameService;
        }

        public virtual string TemplateName { get; set; }

        public virtual string ReportDataSource { get; set; }

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var ilrFileSubmissionsPerDayModel = await _modelBuilder.Build(reportServiceContext, cancellationToken);
            var reportFileName = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Excel, true, true, false);

            await GenerateWorkBookAsync(ilrFileSubmissionsPerDayModel, TemplateName, ReportDataSource, reportServiceContext, reportFileName, cancellationToken);
            return new[] { reportFileName };
        }

        private async Task GenerateWorkBookAsync(
            TModel model,
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
