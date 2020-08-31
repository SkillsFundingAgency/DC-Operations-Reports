using System;
using System.Collections.Generic;
using System.Threading;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.ILRFileSubmissionPerDayReport;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.Operations.Reports.Tests.Reports.ILRFileSubmissionsPerDayReport
{
    public class ILRFileSubmissionsPerDayReportModelBuilderTests
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

            var providerServiceMock = new Mock<IILRFileSubmissionsPerDayProviderService>();
            providerServiceMock.Setup(x => x.GetILRFileSubmissionsPerDay(It.IsAny<int>(), It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(BuildIlrReturns());
           
            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(p => p.ConvertUtcToUk(submissionDateTime)).Returns(ukDateTime);

            var modelBuilder = new ILRFileSubmissionPerDayReportModelBuilder(providerServiceMock.Object, dateTimeProviderMock.Object);

            var result = modelBuilder.Build(reportServiceContextMock.Object, CancellationToken.None).Result;

            result.Period.Should().Be("R13");
            result.ChartTitle.Should().Be("1920 ILR File Submissions per Day per Period (Log 2 Scale) - R13");
            result.ReportTitle.Should().Be("1920 ILR File Submissions per Day per Period - 01 Jan 2020 01:01:01");
            result.IlrFileSubmissionsPerDayList.Count.Should().Be(5);
        }

        private List<ILRFileSubmissionsPerDay> BuildIlrReturns()
        {
            return new List<ILRFileSubmissionsPerDay>
            {
                new ILRFileSubmissionsPerDay()
                {
                    DaysToClose = -25,
                    NumberOfSubmissions = 100
                },
                new ILRFileSubmissionsPerDay()
                {
                    DaysToClose = -24,
                    NumberOfSubmissions = 90
                },
                new ILRFileSubmissionsPerDay()
                {
                    DaysToClose = -23,
                    NumberOfSubmissions = 80
                },
                new ILRFileSubmissionsPerDay()
                {
                    DaysToClose = -22,
                    NumberOfSubmissions = 70
                },
                new ILRFileSubmissionsPerDay()
                {
                    DaysToClose = -21,
                    NumberOfSubmissions = 150
                },
            };
        }
    }
}
