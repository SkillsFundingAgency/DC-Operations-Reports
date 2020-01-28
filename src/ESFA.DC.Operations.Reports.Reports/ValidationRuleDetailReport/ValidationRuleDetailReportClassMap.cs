using CsvHelper.Configuration;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport
{
    public class ValidationRuleDetailReportClassMap : ClassMap<ValidationRuleDetail>
    {
        public ValidationRuleDetailReportClassMap()
        {
            var index = 0;

            Map(m => m.ReturnPeriod).Name(@"Period").Index(++index);
            Map(m => m.UkPrn).Name(@"UKPRN").Index(++index);
            Map(m => m.ProviderName).Name(@"Name").Index(++index);
            Map(m => m.Errors).Name(@"No Of Errors").Index(++index);
            Map(m => m.Warnings).Name(@"No Of Warnings").Index(++index);
            Map(m => m.SubmissionDate).Name(@"Date Submitted").Index(++index);
            Map().Name(@"OFFICIAL-SENSITIVE").Constant(string.Empty).Index(++index);
        }
    }
}
