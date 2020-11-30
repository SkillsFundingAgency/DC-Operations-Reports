using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Reports.Abstract
{
    public abstract class AbstractReport
    {
        protected AbstractReport(string taskName, string fileName)
        {
            TaskName = taskName;
            ReportName = fileName;
        }

        public string TaskName { get; }

        public string ReportName { get; }
    }
}
