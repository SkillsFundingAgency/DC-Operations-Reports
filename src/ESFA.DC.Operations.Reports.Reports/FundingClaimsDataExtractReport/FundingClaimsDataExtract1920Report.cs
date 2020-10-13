using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.FundingClaims;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Abstract;
using ESFA.DC.Operations.Reports.Reports.Constants;

namespace ESFA.DC.Operations.Reports.Reports.FundingClaimsDataExtractReport
{
    public class FundingClaimsDataExtract1920Report : AbstractReport, IReport
    {
        private readonly IExcelFileService _excelFileService;
        private readonly IFileNameService _fileNameService;
        private readonly IFundingClaimsProviderService _fundingClaimsProviderService;
        private readonly IOrgProviderService _orgProviderService;
        private readonly IFundingClaimsDataExtractModelBuilder _modelBuilder;

        public FundingClaimsDataExtract1920Report(
            IExcelFileService excelFileService,
            IFileNameService fileNameService,
            IFundingClaimsProviderService fundingClaimsProviderService,
            IOrgProviderService orgProviderService,
            IFundingClaimsDataExtractModelBuilder modelBuilder)
            : base(ReportTaskNameConstants.FundingClaimsDataExtractReport1920, "1920 Funding Claims Data")
        {
            _excelFileService = excelFileService;
            _fileNameService = fileNameService;
            _fundingClaimsProviderService = fundingClaimsProviderService;
            _orgProviderService = orgProviderService;
            _modelBuilder = modelBuilder;
        }

        private string TemplateName => "FundingClaimsDataExtractReport1920Template.xlsx";

        private string ReportDataSource => "FundingClaimDataExtractInfo";

        private int CollectionYear => 1920;

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var collection = await _fundingClaimsProviderService.GetLatestCollectionDetailAsync(CollectionYear, cancellationToken);
            var fundingClaimsDataExtract = await _fundingClaimsProviderService.GetFundingClaimsDataExtractAsync(collection.CollectionId, cancellationToken);
            var fundingClaimsSubmissionsUkprns = fundingClaimsDataExtract.Select(x => x.Ukprn).Distinct().ToList();

            IDictionary<int, OrgModel> orgDetails = await _orgProviderService.GetOrgDetailsForUKPRNsAsync(fundingClaimsSubmissionsUkprns, cancellationToken);

            var fundingClaimsDataExtractReportModel = _modelBuilder.Build(collection, fundingClaimsDataExtract, orgDetails, cancellationToken);
            var reportFileName = _fileNameService.Generate(reportServiceContext, ReportName, OutputTypes.Excel, true, false, false);

            await GenerateWorkBookAsync(fundingClaimsDataExtractReportModel, TemplateName, ReportDataSource, reportServiceContext, reportFileName, cancellationToken);
            return new[] { reportFileName };
        }

        private async Task GenerateWorkBookAsync(
            FundingClaimsDataExtractReportModel model,
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
