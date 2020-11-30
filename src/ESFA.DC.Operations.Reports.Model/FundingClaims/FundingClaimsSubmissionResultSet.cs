using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model.FundingClaims
{
    public class FundingClaimsSubmissionResultSet
    {
        public Guid SubmissionId { get; set; }

        public long Ukprn { get; set; }

        public int CollectionId { get; set; }

        public int Version { get; set; }

        public bool IsSubmitted { get; set; }

        public bool IsSigned { get; set; }

        public bool? CovidDeclaration { get; set; }

        public DateTime? SubmittedDateTimeUtc { get; set; }

        public int SubmissionValueId { get; set; }

        public Guid SubmissionValueSubmissionId { get; set; }

        public decimal TotalDelivery { get; set; }

        public string SubmissionValueFundingStreamPeriodCode { get; set; }

        public string ContractAllocationNumber { get; set; }

        public int SubmissionContractDetailId { get; set; }

        public string SubmissionContractFundingStreamPeriodCode { get; set; }

        public decimal ContractValue { get; set; }

        public Guid SubmissionContractSubmissionId { get; set; }
    }
}
