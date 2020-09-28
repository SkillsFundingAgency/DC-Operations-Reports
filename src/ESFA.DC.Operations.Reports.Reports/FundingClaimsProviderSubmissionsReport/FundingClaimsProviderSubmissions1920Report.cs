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
using ESFA.DC.Operations.Reports.Interface.FundingClaims;
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
        private readonly IFundingClaimsProviderService _fundingClaimsProviderService;
        private readonly IOrgProviderService _orgProviderService;
        private readonly IFundingClaimsSubmissionsModelBuilder _modelBuilder;

        public FundingClaimsProviderSubmissions1920Report(
            IFileService fileService,
            IJsonSerializationService serializationService,
            IExcelFileService excelFileService,
            IFileNameService fileNameService,
            IOrganisationCollectionProviderService organisationCollectionProviderService,
            IFundingClaimsProviderService fundingClaimsProviderService,
            IOrgProviderService orgProviderService,
            IFundingClaimsSubmissionsModelBuilder modelBuilder)
            : base(ReportTaskNameConstants.FundingClaimsProviderSubmissionsReport1920, "1920 Funding Claims Provider Submissions Report")
        {
            _fileService = fileService;
            _serializationService = serializationService;
            _excelFileService = excelFileService;
            _fileNameService = fileNameService;
            _organisationCollectionProviderService = organisationCollectionProviderService;
            _fundingClaimsProviderService = fundingClaimsProviderService;
            _orgProviderService = orgProviderService;
            _modelBuilder = modelBuilder;
        }

        private string TemplateName => "FundingClaimsProviderSubmissionsReport1920Template.xlsx";

        private string ReportDataSource => "FundingClaimsInfo";

        private int CollectionYear => 1920;

        public async Task<IEnumerable<string>> GenerateAsync(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var collection = await _fundingClaimsProviderService.GetLatestCollectionDetailAsync(CollectionYear, cancellationToken);
            var expectedProviders = await _organisationCollectionProviderService.GetOrganisationCollectionsByCollectionIdAsync(collection.CollectionId, cancellationToken);
            var fundingClaimsSubmissions = await _fundingClaimsProviderService.GetAllFundingClaimsSubmissionsByCollectionAsync(collection.CollectionId, cancellationToken);

            var organisationCollections = expectedProviders.ToList();
            var expectedUkprns = organisationCollections.Select(x => (long)x.Ukprn);

            var fundingClaimsSubmissionsUkprns = fundingClaimsSubmissions.Select(x => x.Ukprn);
            var ukprns = expectedUkprns.Union(fundingClaimsSubmissionsUkprns);

            IDictionary<int, OrgModel> orgDetails = await _orgProviderService.GetOrgDetailsForUKPRNsAsync(ukprns.Distinct().ToList(), CancellationToken.None);

            var fundingClaimsSubmissionsModel = await _modelBuilder.Build(collection, organisationCollections, fundingClaimsSubmissions, orgDetails, cancellationToken);
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
