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
    public class ILRFileSubmissionPerDayReport : AbstractILRSubmissionsReport<ILRFileSubmissionsPerDayModel>
    {
        public ILRFileSubmissionPerDayReport(
            IExcelFileService excelFileService,
            IFileNameService fileNameService,
            IModelBuilder<ILRFileSubmissionsPerDayModel> modelBuilder)
            : base(ReportTaskNameConstants.ILRFileSubmissionsPerDayReport, "ILR File Submissions Per Day Report", modelBuilder, excelFileService, fileNameService)
        {
        }

        public override string TemplateName => "ILRFileSubmissionPerDayReport.xlsx";

        public override string ReportDataSource => "IlrSubmissionsInfo";
    }
}
