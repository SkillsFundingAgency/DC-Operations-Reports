using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ESFA.DC.CollectionsManagement.Models;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.Operations.Reports.Tests.Reports.ValidationRuleDetailReport
{
    public class ValidationRuleDetailReportModelBuilderTests
    {
        [Fact]
        public void BuildModelTest()
        {
            var reportServiceContextMock = new Mock<IOperationsReportServiceContext>(0);
            reportServiceContextMock.SetupGet(x => x.JobId).Returns(1);
            reportServiceContextMock.SetupGet(x => x.Rule).Returns("RULE_1");
            reportServiceContextMock.SetupGet(x => x.SubmissionDateTimeUtc).Returns(DateTime.UtcNow);
            reportServiceContextMock.SetupGet(x => x.SelectedCollectionYear).Returns(1920);
            reportServiceContextMock.SetupGet(x => x.SelectedILRPeriodsAdjustedTimes).Returns(BuildReturnPeriodsModel());

            Mock<IOrgProviderService> orgProviderMock = new Mock<IOrgProviderService>();
            Mock<IIndex<ILRYears, IValidationRuleDetailsProviderService>> validationRuleDetailsProviderMock = new Mock<IIndex<ILRYears, IValidationRuleDetailsProviderService>>();

            var orgInfo = BuildOrgModel();
            var buildValidationRuleDetails = BuildValidationRuleDetails();
            orgProviderMock.Setup(x => x.GetOrgDetailsForUKPRNsAsync(It.IsAny<List<long>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(orgInfo);

            validationRuleDetailsProviderMock.Setup(x =>
                    x[ILRYears.Year1920].GetValidationRuleDetails(It.IsAny<string>(), It.IsAny<IEnumerable<ReturnPeriod>>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(buildValidationRuleDetails);
            var modelBuilder =
                new ValidationRuleDetailReportModelBuilder(validationRuleDetailsProviderMock.Object,
                    orgProviderMock.Object);
            var result = modelBuilder.Build(reportServiceContextMock.Object, CancellationToken.None).Result;

            result.ElementAt(0).ProviderName.Should().Be("Provider 1");
            result.ElementAt(0).UkPrn.Should().Be(123456789);
            result.ElementAt(1).ProviderName.Should().Be("Provider 2");

        }

        private ICollection<ValidationRuleDetail> BuildValidationRuleDetails()
        {
            return new List<ValidationRuleDetail>
            {
                new ValidationRuleDetail
                {
                   UkPrn = 123456789,
                   Warnings = 1,
                   ReturnPeriod = "R01",
                   SubmissionDate = new DateTime(2019,11,01),
                   Errors = 1
                },
                new ValidationRuleDetail
                {
                    UkPrn = 987654321,
                    Warnings = 2,
                    ReturnPeriod = "R02",
                    SubmissionDate = new DateTime(2020,01,01),
                    Errors = 2
                }
            };
        }

        private IDictionary<int, OrgModel> BuildOrgModel()
        {
          return new Dictionary<int, OrgModel>()
            {
                { 123456789, new OrgModel {  Ukprn = 123456789, Name = "Provider 1" } },
                { 987654321, new OrgModel {  Ukprn = 987654321, Name = "Provider 2" } },
            };

        }

        private List<ReturnPeriod> BuildReturnPeriodsModel()
        {
            return new List<ReturnPeriod>
            {
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 08, 22, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2019, 09, 05, 15, 30, 45),
                    PeriodNumber = 1,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 09, 17, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2019, 10, 04, 15, 30, 45),
                    PeriodNumber = 2,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 10, 16, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2019, 11, 06, 15, 30, 45),
                    PeriodNumber = 3,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 11, 18, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2019, 12, 05, 15, 30, 45),
                    PeriodNumber = 4,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2019, 12, 17, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 01, 07, 15, 30, 45),
                    PeriodNumber = 5,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 01, 17, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 02, 06, 15, 30, 45),
                    PeriodNumber = 6,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 02, 18, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 03, 05, 15, 30, 45),
                    PeriodNumber = 7,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 03, 17, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 04, 06, 15, 30, 45),
                    PeriodNumber = 8,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 04, 20, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 05, 06, 15, 30, 45),
                    PeriodNumber = 9,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 05, 19, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 06, 04, 15, 30, 45),
                    PeriodNumber = 10,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 06, 16, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 07, 06, 15, 30, 45),
                    PeriodNumber = 11,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 07, 16, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 08, 06, 15, 30, 45),
                    PeriodNumber = 12,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 08, 07, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 09, 13, 15, 30, 45),
                    PeriodNumber = 13,
                },
                new ReturnPeriod
                {
                    StartDateTimeUtc = new DateTime(2020, 09, 14, 13, 30, 00),
                    EndDateTimeUtc = new DateTime(2020, 10, 17, 15, 30, 45),
                    PeriodNumber = 14,
                },
            };
        }
    }
}
