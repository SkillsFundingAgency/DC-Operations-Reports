using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model.FundingClaims
{
    public class FundingClaimsSubmission
    {
        public Guid SubmissionId { get; set; }

        public long Ukprn { get; set; }

        public int CollectionId { get; set; }

        public int Version { get; set; }

        public bool IsSubmitted { get; set; }

        public DateTime SubmittedDateTimeUtc { get; set; }
    }
}
