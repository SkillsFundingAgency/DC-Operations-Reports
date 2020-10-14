using ESFA.DC.Operations.Reports.Model.FundingClaims;
using System;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders
{
    public class FundingClaimsDataExtractResultSetBuilder : AbstractBuilder<FundingClaimsDataExtractResultSet>
    {
        public Guid SubmissionId = new Guid("08BD2CBD-FB97-447D-860A-FEAB8D03A5EA");

        public const int CollectionId = 174;

        public int Ukprn = 12345678;

        public const byte Declaration = 1;

        public const byte CovidDeclaration = 1;

        public DateTime UpdatedOn = new DateTime(2020, 01, 01);

        public const int SubmissionValueId = 1;

        public const string SubmissionValueFundingStreamPeriodCode = "1619ED1920";

        public const int DeliverableCode = 1001;

        public const string DeliverableDescription = "540+ hours (Band 5)";

        public const int StudentNumbers = 15;

        public const decimal DeliveryToDate = 90.5M;

        public const decimal ForecastedDelivery = 150M;

        public const decimal ExceptionalAdjustments = 20M;

        public const decimal TotalDelivery = 1500.0M;

        public const string ContractAllocationNumber = "ALLC-4391";

        public const decimal ContractValue = 1000;

        public const byte IsSigned  = 1;

        public FundingClaimsDataExtractResultSetBuilder()
        {
            modelObject = new FundingClaimsDataExtractResultSet()
            {
                SubmissionId = SubmissionId,
                CollectionId = CollectionId,
                Ukprn = Ukprn,
                Declaration = Declaration,
                CovidDeclaration = CovidDeclaration,
                UpdatedOn = UpdatedOn,
                SubmissionValueId = SubmissionValueId,
                SubmissionValueFundingStreamPeriodCode = SubmissionValueFundingStreamPeriodCode,
                DeliverableCode = DeliverableCode,
                Description = DeliverableDescription,
                StudentNumbers = StudentNumbers,
                DeliveryToDate = DeliveryToDate,
                ForecastedDelivery = ForecastedDelivery,
                ExceptionalAdjustments = ExceptionalAdjustments,
                TotalDelivery = TotalDelivery,
                ContractAllocationNumber = ContractAllocationNumber,
                ContractValue = ContractValue,
                Signed = IsSigned,
            };
        }
    }
}
