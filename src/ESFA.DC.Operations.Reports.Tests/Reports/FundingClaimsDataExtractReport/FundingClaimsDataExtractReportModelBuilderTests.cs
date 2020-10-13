using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;
using ESFA.DC.Operations.Reports.Reports.FundingClaimsProviderSubmissionsReport;
using ESFA.DC.Operations.Reports.Tests.Builders;
using ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsDataExtractReport
{
    public class FundingClaimsDataExtractReportModelBuilderTests
    {
        [Fact]
        public void BuildModelTest()
        {
            var ukDateTime = new DateTime(2020, 1, 1, 1, 1, 1);
            var reportServiceContextMock = new Mock<IOperationsReportServiceContext>(0);
            reportServiceContextMock.SetupGet(x => x.JobId).Returns(1);
            reportServiceContextMock.SetupGet(x => x.CollectionYear).Returns(1920);
            reportServiceContextMock.SetupGet(x => x.Period).Returns(13);
            reportServiceContextMock.SetupGet(x => x.ReturnPeriodName).Returns("R13");

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(p => p.ConvertUtcToUk(It.IsAny<DateTime>())).Returns(ukDateTime);

            var collectionDetail = new CollectionDetailBuilder().Build();
            var org1 = new OrganisationCollectionBuilder().With(x => x.Ukprn, 12345678).Build();
            var org2 = new OrganisationCollectionBuilder().With(x => x.Ukprn, 87654321).Build();
            var org3 = new OrganisationCollectionBuilder().With(x => x.Ukprn, 11111111).Build();
            var org4 = new OrganisationCollectionBuilder().With(x => x.Ukprn, 99999999).Build();

            var expectedProviders = new List<OrganisationCollection>()
            {
                org1, org2, org3, org4
            };

            var fundingClaimsDataExtractResultSet1 = new FundingClaimsDataExtractResultSetBuilder().With(x => x.Ukprn, 12345678).Build();
            var fundingClaimsDataExtractResultSet2 = new FundingClaimsDataExtractResultSetBuilder()
                                                                                                    .With(x => x.Ukprn, 87654321)
                                                                                                    .With(x => x.DeliverableCode, 1002)
                                                                                                    .With(x => x.Description, "Up to 279 hours (Band 1)")
                                                                                                    .With(x => x.StudentNumbers, 21)
                                                                                                    .With(x => x.SubmissionValueFundingStreamPeriodCode, "AEBC-19TRN1920")
                                                                                                    .With(x => x.ContractAllocationNumber, "16ED - 1167")
                                                                                                    .With(x => x.TotalDelivery, 10000).Build();

            var fundingClaimsDataExtractResultSets = new List<FundingClaimsDataExtractResultSet>()
            {
                fundingClaimsDataExtractResultSet1,
                fundingClaimsDataExtractResultSet2
            };

            var orgModel1 = new OrgModelBuilder().With(x => x.Ukprn, 12345678).With(x => x.Name, "Provider1").Build();
            var orgModel2 = new OrgModelBuilder().With(x => x.Ukprn, 87654321).With(x => x.Name, "Provider2").Build();
            
            var orgDetails = new Dictionary<int, OrgModel>()
            {
                {12345678, orgModel1 },
                {87654321, orgModel2 }
            };

            var result = NewBuilder(dateTimeProviderMock.Object).Build(collectionDetail, fundingClaimsDataExtractResultSets, orgDetails, CancellationToken.None);
            result.FundingClaimsDataExtract[0].Ukprn.Should().Be(12345678);
            result.FundingClaimsDataExtract[0].ProviderName.Should().Be("Provider1");
            result.FundingClaimsDataExtract[0].CollectionPeriod.Should().Be("FC03");
            result.FundingClaimsDataExtract[0].Declaration.Should().Be(1);
            result.FundingClaimsDataExtract[0].CovidDeclaration.Should().Be(1);
            result.FundingClaimsDataExtract[0].UpdatedOn.Should().Be("01/01/2020 00:00:00");
            result.FundingClaimsDataExtract[0].FundingStreamPeriodCode.Should().Be("1619ED1920");
            result.FundingClaimsDataExtract[0].DeliverableCode.Should().Be(1001);
            result.FundingClaimsDataExtract[0].DeliverableDescription.Should().Be("540+ hours (Band 5)");
            result.FundingClaimsDataExtract[0].StudentNumbers.Should().Be(15);
            result.FundingClaimsDataExtract[0].DeliveryToDate.Should().Be(90.5M);
            result.FundingClaimsDataExtract[0].ForecastedDelivery.Should().Be(150M);
            result.FundingClaimsDataExtract[0].ExceptionalAdjustments.Should().Be(20M);
            result.FundingClaimsDataExtract[0].TotalDelivery.Should().Be(1500M);
            result.FundingClaimsDataExtract[0].ContractAllocationNumber.Should().Be("ALLC-4391");
            result.FundingClaimsDataExtract[0].MaximumContractValue.Should().Be(1000);


            result.FundingClaimsDataExtract[1].Ukprn.Should().Be(87654321);
            result.FundingClaimsDataExtract[1].ProviderName.Should().Be("Provider2");
            result.FundingClaimsDataExtract[1].CollectionPeriod.Should().Be("FC03");
            result.FundingClaimsDataExtract[1].CovidDeclaration.Should().Be(1);
            result.FundingClaimsDataExtract[1].FundingStreamPeriodCode.Should().Be("AEBC-19TRN1920");
            result.FundingClaimsDataExtract[1].DeliverableCode.Should().Be(1002);
            result.FundingClaimsDataExtract[1].DeliverableDescription.Should().Be("Up to 279 hours (Band 1)");
            result.FundingClaimsDataExtract[1].StudentNumbers.Should().Be(21);
            result.FundingClaimsDataExtract[1].TotalDelivery.Should().Be(10000);
            result.FundingClaimsDataExtract[1].ContractAllocationNumber.Should().Be("16ED - 1167");
        }

        private FundingClaimsDataExtract1920ReportModelBuilder NewBuilder(
            IDateTimeProvider dateTimeProvider = null)
        {
            return new FundingClaimsDataExtract1920ReportModelBuilder(dateTimeProvider ?? Mock.Of<IDateTimeProvider>());
        }
    }
}
