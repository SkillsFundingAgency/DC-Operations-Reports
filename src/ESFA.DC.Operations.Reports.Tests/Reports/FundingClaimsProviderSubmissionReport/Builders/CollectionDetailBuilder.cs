using ESFA.DC.Operations.Reports.Model.FundingClaims;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders
{
    public class CollectionDetailBuilder : AbstractBuilder<CollectionDetail>
    {
        public const int CollectionId = 174;

        public const int CollectionYear = 1920;

        public const string CollectionName = "Funding Claims 1920 Final";

        public const string DisplayTitle = "Funding Claims Display Title";

        public CollectionDetailBuilder()
        {
            modelObject = new CollectionDetail()
            {
               CollectionId = CollectionId,
               CollectionYear = CollectionYear,
               CollectionName = CollectionName,
               DisplayTitle = DisplayTitle,
            };
        }
    }
}
