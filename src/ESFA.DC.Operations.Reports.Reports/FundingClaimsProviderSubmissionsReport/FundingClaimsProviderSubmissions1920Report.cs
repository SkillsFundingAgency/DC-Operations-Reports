using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FileService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Abstract;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Serialization.Interfaces;

namespace ESFA.DC.Operations.Reports.Reports.FundingClaimsProviderSubmissionsReport
{
    public class FundingClaimsProviderSubmissions1920Report : AbstractReport, IReport
    {
        private readonly IFileService _fileService;
        private readonly IJsonSerializationService _serializationService;
        private readonly IExcelFileService _excelFileService;
        private readonly IFileNameService _fileNameService;
        private readonly IOrganisationCollectionProviderService _organisationCollectionProviderService;
        private readonly IModelBuilder<FundingClaimsSubmissionsModel> _modelBuilder;

        public FundingClaimsProviderSubmissions1920Report(
            IFileService fileService,
            IJsonSerializationService serializationService,
            IExcelFileService excelFileService,
            IFileNameService fileNameService,
            IOrganisationCollectionProviderService organisationCollectionProviderService,
            IModelBuilder<FundingClaimsSubmissionsModel> modelBuilder)
            : base(ReportTaskNameConstants.FundingClaimsProviderSubmissionsReport1920, "1920 Funding Claims Provider Submissions Report")
        {
            _fileService = fileService;
            _serializationService = serializationService;
            _excelFileService = excelFileService;
            _fileNameService = fileNameService;
            _organisationCollectionProviderService = organisationCollectionProviderService;
            _modelBuilder = modelBuilder;
        }

        private string TemplateName => "FundingClaimsProviderSubmissionsReport1920Template.xlsx";

        private string ReportDataSource => "FundingClaimsInfo";


        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var expectedProviders = await  _organisationCollectionProviderService.GetOrganisationCollectionsByCollectionIdAsync(174, cancellationToken);
            var fundingClaimsSubmissionsModel = await _modelBuilder.Build(reportServiceContext, cancellationToken);
            var reportFileName = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Excel, true, true, false);

            await GenerateWorkBookAsync(fundingClaimsSubmissionsModel, TemplateName, ReportDataSource, reportServiceContext, reportFileName, cancellationToken);
            return new[] { reportFileName };
        }

        private async Task GenerateWorkBookAsync(
            FundingClaimsSubmissionsModel model,
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
