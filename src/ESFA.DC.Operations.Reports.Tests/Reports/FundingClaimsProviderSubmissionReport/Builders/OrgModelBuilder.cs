using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders
{
    public class OrgModelBuilder : AbstractBuilder<OrgModel>
    {
        public const int Ukprn = 12345678;

        public const string Name = "Provder1";

        public OrgModelBuilder()
        {
            modelObject = new OrgModel()
            {
                Ukprn = Ukprn,
                Name = Name,
            };
        }
    }
}
