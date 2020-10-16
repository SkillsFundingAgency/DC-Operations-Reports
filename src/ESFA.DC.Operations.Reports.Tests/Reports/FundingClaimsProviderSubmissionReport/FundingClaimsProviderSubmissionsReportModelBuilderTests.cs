using System;
using System.Collections.Generic;
using System.Threading;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Model.FundingClaims;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Operations.Reports.Reports.FundingClaimsProviderSubmissionsReport;
using ESFA.DC.Operations.Reports.Tests.Builders;
using ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport.Builders;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.Operations.Reports.Tests.Reports.FundingClaimsProviderSubmissionReport
{
    public class FundingClaimsProviderSubmissionsReportModelBuilderTests
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

            var fundingClaimSubmission1 = new FundingClaimsSubmissionBuilder().With(x => x.Ukprn, 12345678).With(x => x.IsSigned, false).Build();
            var fundingClaimSubmission2 = new FundingClaimsSubmissionBuilder(new Guid("18BD2CBD-FB97-447D-860A-FEAB8D03A5EA")).With(x => x.Ukprn, 87654321).Build();

            var fundingClaimsSubmissions = new List<FundingClaimsSubmission>()
            {
                fundingClaimSubmission1,
                fundingClaimSubmission2
            };

            var orgModel1 = new OrgModelBuilder().With(x => x.Ukprn, 12345678).With(x => x.Name, "Provider1").Build();
            var orgModel2 = new OrgModelBuilder().With(x => x.Ukprn, 87654321).With(x => x.Name, "Provider2").Build();
            var orgModel3 = new OrgModelBuilder().With(x => x.Ukprn, 11111111).With(x => x.Name, "Provider3").Build();
            var orgModel4 = new OrgModelBuilder().With(x => x.Ukprn, 99999999).With(x => x.Name, "Provider4").Build();

            var orgDetails = new Dictionary<int, OrgModel>()
            {
                {12345678, orgModel1 },
                {87654321, orgModel2 },
                {11111111, orgModel3 },
                {99999999, orgModel4 },
            };

            var result = NewBuilder(dateTimeProviderMock.Object).Build(collectionDetail, expectedProviders, fundingClaimsSubmissions, orgDetails, CancellationToken.None);

            result.FundingClaim.Should().Be("Funding Claims Display Title");
            result.FundingClaimsSubmissionsDetails.Count.Should().Be(4);
            result.NoOfExpectedProvidersNotReturning.Should().Be(2);
            result.NoOfProvidersExpectedToReturn.Should().Be(4);
            result.NoOfReturningExpectedProviders.Should().Be(2);
            result.NoOfReturningUnexpectedProviders.Should().Be(0);
            result.TotalNoOfReturningProviders.Should().Be(2);
            result.ReportRun.Should().Be("01/01/2020 01:01:01");

            result.FundingClaimsSubmissionsDetails[0].ProviderName.Should().Be("Provider1");
            result.FundingClaimsSubmissionsDetails[0].ExpectedToReturnInCurrentPeriod.Should().Be("Yes");
            result.FundingClaimsSubmissionsDetails[0].Signed.Should().Be("No");
            result.FundingClaimsSubmissionsDetails[0].AEB19TRLS1920ProcuredClaimed.Should().Be(50);
            result.FundingClaimsSubmissionsDetails[0].AEB19TRLS1920ProcuredContractValue.Should().Be(51);
            result.FundingClaimsSubmissionsDetails[0].AEBASLS1920ProcuredClaimed.Should().Be(40);
            result.FundingClaimsSubmissionsDetails[0].AEBASLS1920ProcuredContractValue.Should().Be(41);
            result.FundingClaimsSubmissionsDetails[0].AEBC19TRN1920Claimed.Should().Be(30);
            result.FundingClaimsSubmissionsDetails[0].AEBC19TRN1920ContractValue.Should().Be(31);
            result.FundingClaimsSubmissionsDetails[0].AEBCASCL1920Claimed.Should().Be(20);
            result.FundingClaimsSubmissionsDetails[0].AEBCASCL1920ContractValue.Should().Be(21);
            result.FundingClaimsSubmissionsDetails[0].ALLBC1920Claimed.Should().Be(10);
            result.FundingClaimsSubmissionsDetails[0].ALLBC1920ContractValue.Should().Be(11);
            result.FundingClaimsSubmissionsDetails[0].ED1920Claimed1619.Should().Be(60);
            result.FundingClaimsSubmissionsDetails[0].ED1920ContractValue1619.Should().Be(61);

            result.FundingClaimsSubmissionsDetails[1].ProviderName.Should().Be("Provider2");
            result.FundingClaimsSubmissionsDetails[1].Signed.Should().Be("Yes");
            result.FundingClaimsSubmissionsDetails[1].AEB19TRLS1920ProcuredClaimed.Should().Be(50);
            result.FundingClaimsSubmissionsDetails[1].AEB19TRLS1920ProcuredContractValue.Should().Be(51);
            result.FundingClaimsSubmissionsDetails[1].AEBASLS1920ProcuredClaimed.Should().Be(40);
            result.FundingClaimsSubmissionsDetails[1].AEBASLS1920ProcuredContractValue.Should().Be(41);

            result.FundingClaimsSubmissionsDetails[2].ProviderName.Should().Be("Provider3");
            result.FundingClaimsSubmissionsDetails[2].AEB19TRLS1920ProcuredClaimed.Should().Be(0);
            result.FundingClaimsSubmissionsDetails[2].AEB19TRLS1920ProcuredContractValue.Should().Be(0);
            result.FundingClaimsSubmissionsDetails[2].AEBASLS1920ProcuredClaimed.Should().Be(0);
            result.FundingClaimsSubmissionsDetails[2].AEBASLS1920ProcuredContractValue.Should().Be(0);

            result.FundingClaimsSubmissionsDetails[3].ProviderName.Should().Be("Provider4");
            result.FundingClaimsSubmissionsDetails[3].AEB19TRLS1920ProcuredClaimed.Should().Be(0);
            result.FundingClaimsSubmissionsDetails[3].AEB19TRLS1920ProcuredContractValue.Should().Be(0);
            result.FundingClaimsSubmissionsDetails[3].AEBASLS1920ProcuredClaimed.Should().Be(0);
            result.FundingClaimsSubmissionsDetails[3].AEBASLS1920ProcuredContractValue.Should().Be(0);
        }

        [Fact]
        public void GetClaimedValue_Returns_ExpectedValue()
        {
            var submissionId = new Guid("18BD2CBD-FB97-447D-860A-FEAB8D03A5EA");
            var submissionValues = new List<FundingClaimSubmissionsValue>
                {
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.ALLBC1920).With(x => x.TotalDelivery, 10).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBCASCL1920).With(x => x.TotalDelivery, 20).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBC19TRN1920).With(x => x.TotalDelivery, 30).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBASLS1920).With(x => x.TotalDelivery, 40).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEB19TRLS1920).With(x => x.TotalDelivery, 50).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionsValueBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.C1619ED1920).With(x => x.TotalDelivery, 60).With(x => x.SubmissionId, submissionId).Build(),
                };

            var allbc1920ClaimValue = NewBuilder().GetClaimedValue(submissionId, FundingStreamPeriodCodeConstants.ALLBC1920, submissionValues);
            var c1619ED1920ClaimValue = NewBuilder().GetClaimedValue(submissionId, FundingStreamPeriodCodeConstants.C1619ED1920, submissionValues);
            var nonExistentFSPerioCodeValue = NewBuilder().GetClaimedValue(submissionId, "NonExistentFSPeriodCode", submissionValues);
            var nonExistentSubmissionId = NewBuilder().GetClaimedValue(new Guid(), FundingStreamPeriodCodeConstants.ALLBC1920, submissionValues);

            allbc1920ClaimValue.Should().Be(10);
            c1619ED1920ClaimValue.Should().Be(60);
            nonExistentFSPerioCodeValue.Should().Be(0);
            nonExistentSubmissionId.Should().Be(0);
        }

        [Fact]
        public void GetContractValue_Returns_ExpectedValue()
        {
            var submissionId = new Guid("18BD2CBD-FB97-447D-860A-FEAB8D03A5EA");
            var submissionContractDetailsValues = new List<FundingClaimSubmissionContractDetail>
                {
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.ALLBC1920).With(x => x.ContractValue, 11).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBCASCL1920).With(x => x.ContractValue, 21).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBC19TRN1920).With(x => x.ContractValue, 31).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEBASLS1920).With(x => x.ContractValue, 41).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.AEB19TRLS1920).With(x => x.ContractValue, 51).With(x => x.SubmissionId, submissionId).Build(),
                    new FundingClaimSubmissionContractDetailBuilder().With(x => x.FundingStreamPeriodCode, FundingStreamPeriodCodeConstants.C1619ED1920).With(x => x.ContractValue, 61).With(x => x.SubmissionId, submissionId).Build(),
                };

            var allbc1920ClaimValue = NewBuilder().GetContractValue(submissionId, FundingStreamPeriodCodeConstants.ALLBC1920, submissionContractDetailsValues);
            var c1619ED1920ClaimValue = NewBuilder().GetContractValue(submissionId, FundingStreamPeriodCodeConstants.C1619ED1920, submissionContractDetailsValues);
            var nonExistentFSPerioCodeValue = NewBuilder().GetContractValue(submissionId, "NonExistentFSPeriodCode", submissionContractDetailsValues);

            allbc1920ClaimValue.Should().Be(11);
            c1619ED1920ClaimValue.Should().Be(61);
            nonExistentFSPerioCodeValue.Should().Be(0);
        }

        [Fact]
        public void IsExpectedToReturn_Returns_ExpectedValue()
        {
            var expectedProviders = new List<OrganisationCollection>()
            {
                new OrganisationCollectionBuilder().With(x => x.Ukprn, 12345678).Build(),
                new OrganisationCollectionBuilder().With(x => x.Ukprn, 87654321).Build(),
                new OrganisationCollectionBuilder().With(x => x.Ukprn, 11111111).Build(),
                new OrganisationCollectionBuilder().With(x => x.Ukprn, 99999999).Build(),
            };

            NewBuilder().IsExpectedToReturn(12345678, expectedProviders).Should().Be("Yes");
            NewBuilder().IsExpectedToReturn(87654321, expectedProviders).Should().Be("Yes");
            NewBuilder().IsExpectedToReturn(55555555, expectedProviders).Should().Be("No");
            NewBuilder().IsExpectedToReturn(99999999, expectedProviders).Should().Be("Yes");
        }


        [Theory]
        [InlineData(null, "N/A")]
        [InlineData(true, "Yes")]
        [InlineData(false, "No")]
        public void BuildCovidResponse_Returns_ExpectedValue(bool? covidDeclaration, string expectedValue)
        {
            NewBuilder().BuildCovidResponse(covidDeclaration).Should().Be(expectedValue);
        }

        [Theory]
        [InlineData(true, true, "Yes")]
        [InlineData(true, false, "")]
        [InlineData(false, true, "No")]
        [InlineData(false, false, "")]
        public void BuildSignedResponse_Returns_ExpectedValue(bool isSigned, bool isSubmitted, string expectedValue)
        {
            NewBuilder().BuildSignedResponse(isSigned, isSubmitted).Should().Be(expectedValue);
        }

        private FundingClaimsProviderSubmissions1920ReportModelBuilder NewBuilder(
           IDateTimeProvider dateTimeProvider = null)
        {
            return new FundingClaimsProviderSubmissions1920ReportModelBuilder(dateTimeProvider ?? Mock.Of<IDateTimeProvider>());
        }
    }
}
