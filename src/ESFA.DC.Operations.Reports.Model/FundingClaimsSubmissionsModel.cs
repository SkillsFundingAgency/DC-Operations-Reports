using System.Collections.Generic;

namespace ESFA.DC.Operations.Reports.Model
{
    public class FundingClaimsSubmissionsModel
    {
        public string FundingClaim { get; set; }

        public string ReportRun { get; set; }

        public int TotalNoOfProviders { get; set; }

        public int NoOfProvidersExpectedToReturn { get; set; }

        public int NoOfExpectedProvidersNotReturning { get; set; }

        public int NoOfReturningExpectedProviders { get; set; }

        public int NoOfReturningUnexpectedProviders { get; set; }

        public List<FundingClaimsSubmissionsDetail> FundingClaimsSubmissionsDetails { get; set; }
    }
}
