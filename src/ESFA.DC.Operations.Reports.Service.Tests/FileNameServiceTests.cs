using System;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Service.Providers;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.Operations.Reports.Service.Tests
{
    public class FileNameServiceTests
    {
        [InlineData(OutputTypes.Csv, "csv")]
        [InlineData(OutputTypes.Excel, "xlsx")]
        [InlineData(OutputTypes.Json, "json")]
        [Theory]
        public void GetExtensions(OutputTypes outputType, string extension)
        {
            NewService().GetExtension(outputType).Should().Be(extension);
        }

        [InlineData(1920, "R13", "1920/R13/")]
        [InlineData(2021, "R01", "2021/R01/")]
        [Theory]
        public void GetPrefix_MeetsExpectation(int collectionYear, string period, string expected)
        {
            var reportServiceContextMock = new Mock<IOperationsReportServiceContext>();
            reportServiceContextMock.SetupGet(x => x.CollectionYear).Returns(collectionYear);
            reportServiceContextMock.SetupGet(x => x.ReturnPeriodName).Returns(period);

            NewService().GetPrefix(reportServiceContextMock.Object).Should().Be(expected);
        }

        [Fact]
        public void Generate_MeetsExpectation()
        {
            var submissionDateTime = new DateTime(2019, 1, 1, 1, 1, 1);
            var ukDateTime = new DateTime(2020, 1, 1, 1, 1, 1);
            var ukprn = 1234;
            var jobId = 5678;
            var fileName = "FileName";

            var reportServiceContextMock = new Mock<IOperationsReportServiceContext>();
            reportServiceContextMock.SetupGet(x => x.CollectionYear).Returns(1920);
            reportServiceContextMock.SetupGet(x => x.ReturnPeriodName).Returns("R13");
            reportServiceContextMock.SetupGet(c => c.SubmissionDateTimeUtc).Returns(submissionDateTime);
            reportServiceContextMock.Setup(x => x.JobId).Returns(1234);

            var dateTimeProviderMock = new Mock<IDateTimeProvider>();
            dateTimeProviderMock.Setup(p => p.ConvertUtcToUk(submissionDateTime)).Returns(ukDateTime);

            var result = NewService(dateTimeProviderMock.Object).Generate(reportServiceContextMock.Object, "reportname", OutputTypes.Csv, includeDateTime: true, includeYearPeriodAndShortCode: true, includeJobId: true);
            result.Should().Be("Reports/1234/1920/R13/reportname 20200101-010101.csv");
        }

        private FileNameService NewService(IDateTimeProvider dateTimeProvider = null)
        {
            return new FileNameService(dateTimeProvider);
        }
    }
}