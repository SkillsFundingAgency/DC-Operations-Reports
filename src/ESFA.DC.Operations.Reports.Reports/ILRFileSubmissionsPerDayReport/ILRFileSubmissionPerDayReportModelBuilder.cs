using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Abstract;

namespace ESFA.DC.Operations.Reports.Reports.ILRFileSubmissionPerDayReport
{
    public class ILRFileSubmissionPerDayReportModelBuilder : AbstractILRSubmissionsModelBuilder<ILRFileSubmissionsPerDayModel>, IModelBuilder<ILRFileSubmissionsPerDayModel>
    {
        public ILRFileSubmissionPerDayReportModelBuilder(IILRFileSubmissionsPerDayProviderService ilrFileSubmissionsPerDayService, IDateTimeProvider dateTimeProvider)
            : base(ilrFileSubmissionsPerDayService, dateTimeProvider)
        {
        }

        public override string Title => "ILR File Submissions per Day per Period";
    }
}
