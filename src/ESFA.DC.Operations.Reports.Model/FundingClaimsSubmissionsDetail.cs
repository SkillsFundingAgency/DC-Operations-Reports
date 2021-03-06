﻿namespace ESFA.DC.Operations.Reports.Model
{
    public class FundingClaimsSubmissionsDetail
    {
        public long UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string Signed { get; set; }

        public string CovidResponse { get; set; }

        public string ExpectedToReturnInCurrentPeriod { get; set; }

        public string ReturnedInCurrentPeriod { get; set; }

        public string DateLatestClaimSubmitted { get; set; }

        public decimal ALLBC1920ContractValue { get; set; }

        public decimal ALLBC1920Claimed { get; set; }

        public decimal AEBCASCL1920ContractValue { get; set; }

        public decimal AEBCASCL1920Claimed { get; set; }

        public decimal AEBC19TRN1920ContractValue { get; set; }

        public decimal AEBC19TRN1920Claimed { get; set; }

        public decimal AEBASLS1920ProcuredContractValue { get; set; }

        public decimal AEBASLS1920ProcuredClaimed { get; set; }

        public decimal AEB19TRLS1920ProcuredContractValue { get; set; }

        public decimal AEB19TRLS1920ProcuredClaimed { get; set; }

        public decimal ED1920ContractValue1619 { get; set; }

        public decimal ED1920Claimed1619 { get; set; }
    }
}
