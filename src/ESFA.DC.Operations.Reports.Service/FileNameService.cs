using System.Collections.Generic;
using System.Text;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.Operations.Reports.Interface;

namespace ESFA.DC.Operations.Reports.Service
{
    public class FileNameService : IFileNameService
    {
        private const string Year = @"2019-20";

        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IDictionary<OutputTypes, string> _extensionsDictionary = new Dictionary<OutputTypes, string>()
        {
            [OutputTypes.Csv] = "csv",
            [OutputTypes.Excel] = "xlsx",
            [OutputTypes.Json] = "json",
            [OutputTypes.Zip] = "zip",
        };

        public FileNameService(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public string Generate(IOperationsReportServiceContext reportServiceContext, string fileName, OutputTypes outputType, bool includeDateTime = true, bool includeYearPeriodAndShortCode = true)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(GetPath(reportServiceContext));

            if (includeYearPeriodAndShortCode)
            {
                stringBuilder.Append(GetPrefix(reportServiceContext));
                stringBuilder.Append(" ");
            }

            stringBuilder.Append(fileName);

            if (includeDateTime)
            {
                stringBuilder.Append($" {_dateTimeProvider.ConvertUtcToUk(reportServiceContext.SubmissionDateTimeUtc):yyyyMMdd-HHmmss}");
            }

            stringBuilder.Append($".{GetExtension(outputType)}");

            return stringBuilder.ToString();
        }

        protected virtual string GetPath(IOperationsReportServiceContext reportServiceContext) => $"R{reportServiceContext.Period:D2}/";

        public string GetExtension(OutputTypes outputType) => _extensionsDictionary[outputType];

        public string GetPrefix(IOperationsReportServiceContext reportServiceContext) => $"{Year} R{reportServiceContext.Period:D2}";

    }
}
