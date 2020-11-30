using System;

namespace ESFA.DC.Operations.Reports.Model
{
    public class ValidationRuleDetail
    {
        public string ReturnPeriod { get; set; }

        public string ProviderName { get; set; }

        public int? UkPrn { get; set; }

        public int Errors { get; set; }

        public int Warnings { get; set; }

        public DateTime SubmissionDate { get; set; }
    }
}
