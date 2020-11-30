using ESFA.DC.ExcelService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Abstract;
using ESFA.DC.Operations.Reports.Reports.Constants;

namespace ESFA.DC.Operations.Reports.Reports.ILRProvidersReturningFirstTimePerDayReport
{
    public class ILRProvidersReturningFirstTimePerDayReport : AbstractILRSubmissionsReport<ILRProvidersReturningFirstTimePerDayModel>
    {
       public ILRProvidersReturningFirstTimePerDayReport(
            IExcelFileService excelFileService,
            IFileNameService fileNameService,
            IModelBuilder<ILRProvidersReturningFirstTimePerDayModel> modelBuilder)
            : base(ReportTaskNameConstants.ILRProvidersReturningFirstTimePerDayReport, "ILR Providers Returning First Time Per Day Report", modelBuilder, excelFileService, fileNameService)
        {
        }

       public override string TemplateName => "ILRProvidersReturningFirstTimePerDayTemplate.xlsx";

       public override string ReportDataSource => "IlrReturningProvidersInfo";
    }
}
