using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model
{
    public class FundingClaimsDataExtractReportModel
    {
        public ICollection<FundingClaimsDataExtractDetail> FundingClaimsDataExtract { get; set; }
    }
}
