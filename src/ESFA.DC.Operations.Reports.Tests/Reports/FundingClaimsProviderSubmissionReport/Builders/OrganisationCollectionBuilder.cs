using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders
{
    public class OrganisationCollectionBuilder : AbstractBuilder<OrganisationCollection>
    {
        public const int OrganisationId = 1;

        public const int CollectionId = 174;

        public const int Ukprn = 12345678;

        public OrganisationCollectionBuilder()
        {
            modelObject = new OrganisationCollection()
            {
                OrganisationId = OrganisationId,
                CollectionId = CollectionId,
                Ukprn = Ukprn
            };
        }
    }
}
