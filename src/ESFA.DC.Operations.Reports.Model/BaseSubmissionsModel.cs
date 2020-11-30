using System.Collections.Generic;

namespace ESFA.DC.Operations.Reports.Model
{
    public class BaseSubmissionsModel
    {
        public string ReportTitle { get; set; }

        public string ChartTitle { get; set; }

        public string Period { get; set; }

        public List<SubmissionsPerDay> SubmissionsPerDayList { get; set; }
    }
}
