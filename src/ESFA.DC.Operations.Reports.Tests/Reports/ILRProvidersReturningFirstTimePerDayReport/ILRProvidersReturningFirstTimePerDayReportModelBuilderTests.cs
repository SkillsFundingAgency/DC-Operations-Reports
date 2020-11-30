using System;
using System.Collections.Generic;
using System.Threading;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.ILRProvidersReturningFirstTimePerDayReport;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.Operations.Reports.Tests.Reports.ILRProvidersReturningFirstTimePerDayReport
{
    public class ILRProvidersReturningFirstTimePerDayReportModelBuilderTests
    {
        [Fact]
        public void BuildModelTest()
        {
            var submissionDateTime = new DateTime(2019, 1, 1, 1, 1, 1);
            var ukDateTime = new DateTime(2020, 1, 1, 1, 1, 1);
            var reportServiceContextMock = new Mock<IOperationsReportServiceContext>(0);
            reportServiceContextMock.SetupGet(x => x.JobId).Returns(1);
            reportServiceContextMock.SetupGet(x => x.SubmissionDateTimeUtc).Returns(submissionDateTime);
            reportServiceContextMock.SetupGet(x => x.CollectionYear).Returns(1920);
            reportServiceContextMock.SetupGet(x => x.Period).Returns(13);
            reportServiceContextMock.SetupGet(x => x.ReturnPeriodName).Returns("R13");

            var providerServiceMock = new Mock<IILRProvidersReturningFirstTimePerDayProviderService>();
            providerServiceMock.Setup(x => x.GetSubmissionsPerDay(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(BuildIlrReturns());

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(p => p.ConvertUtcToUk(submissionDateTime)).Returns(ukDateTime);

            ILRProvidersReturningFirstTimePerDayReportModelBuilder modelBuilder = new ILRProvidersReturningFirstTimePerDayReportModelBuilder(providerServiceMock.Object, dateTimeProviderMock.Object);

            var result = modelBuilder.Build(reportServiceContextMock.Object, CancellationToken.None).Result;

            result.Period.Should().Be("R13");
            result.ChartTitle.Should().Be("1920 ILR Providers Returning for the First Time Per Period (Log 2 Scale) - R13");
            result.ReportTitle.Should().Be("1920 ILR Providers Returning for the First Time Per Period - 01 Jan 2020 01:01:01");
            result.SubmissionsPerDayList.Count.Should().Be(5);
        }

        private List<SubmissionsPerDay> BuildIlrReturns()
        {
            return new List<SubmissionsPerDay>
            {
                new SubmissionsPerDay()
                {
                    DaysToClose = -25,
                    NumberOfSubmissions = 100
                },
                new SubmissionsPerDay()
                {
                    DaysToClose = -24,
                    NumberOfSubmissions = 90
                },
                new SubmissionsPerDay()
                {
                    DaysToClose = -23,
                    NumberOfSubmissions = 80
                },
                new SubmissionsPerDay()
                {
                    DaysToClose = -22,
                    NumberOfSubmissions = 70
                },
                new SubmissionsPerDay()
                {
                    DaysToClose = -21,
                    NumberOfSubmissions = 150
                },
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


    }
}
