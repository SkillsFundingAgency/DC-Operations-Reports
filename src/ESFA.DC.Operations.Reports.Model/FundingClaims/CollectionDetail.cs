using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model.FundingClaims
{
    public class CollectionDetail
    {
        public int CollectionId { get; set; }

        public int CollectionYear { get; set; }

        public string CollectionName { get; set; }

        public DateTime SubmissionOpenDateUtc { get; set; }

        public DateTime SubmissionCloseDateUtc { get; set; }

        public string DisplayTitle { get; set; }

        public string CollectionCode { get; set; }
    }
}
