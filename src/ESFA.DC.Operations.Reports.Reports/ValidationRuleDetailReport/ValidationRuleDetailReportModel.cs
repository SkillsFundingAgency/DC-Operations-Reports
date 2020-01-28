using System;

namespace ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport
{
    public class ValidationRuleDetailReportModel
    {
        public string ReturnPeriod { get; set; }

        public string ProviderName { get; set; }

        public string UkPrn { get; set; }

        public int Errors { get; set; }

        public int Warnings { get; set; }

        public DateTime SubmissionDate { get; set; }
    }
}
