using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.Operations.Reports.Tests.Reports.ValidationRuleDetailReport
{
    public class ValidationRuleDetailClassMapTests
    {
        [Fact]
        public void Map_Columns()
        {
            var orderedColumns = new string[]
            {
                "Period",
                "UKPRN",
                "Name",
                "No Of Errors",
                "No Of Warnings",
                "Date Submitted",
                "OFFICIAL-SENSITIVE"
            };

            var input = new List<ValidationRuleDetail>();

            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 8096, true))
                {
                    using (var csvWriter = new CsvWriter(streamWriter))
                    {
                        csvWriter.Configuration.RegisterClassMap<ValidationRuleDetailReportClassMap>();

                        csvWriter.WriteRecords(input);
                    }
                }

                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvReader(streamReader))
                    {
                        csvReader.Read();
                        csvReader.ReadHeader();
                        var header = csvReader.Context.HeaderRecord;
                        header.Should().ContainInOrder(orderedColumns);
                        header.Should().HaveCount(7);
                    }
                }
            }
        }

        [Fact]
        public void ClassMap_Model()
        {
            var input = new List<ValidationRuleDetail>()
            {
                new ValidationRuleDetail()
                {
                    ReturnPeriod = "R04",
                    UkPrn = 123456789,
                    ProviderName = "Test",
                    SubmissionDate = new DateTime(2020,01,01),
                    Errors = 12,
                    Warnings = 10
                }
            };

            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 8096, true))
                {
                    using (var csvWriter = new CsvWriter(streamWriter))
                    {
                        csvWriter.Configuration.RegisterClassMap<ValidationRuleDetailReportClassMap>();

                        csvWriter.WriteRecords(input);
                    }
                }

                stream.Position = 0;

                using (var streamReader = new StreamReader(stream))
                {
                    using (var csvReader = new CsvReader(streamReader))
                    {
                        var output = csvReader.GetRecords<dynamic>().ToList();

                        (output[0] as IDictionary<string, object>).Values.ToArray()[0].Should().Be("R04");
                        (output[0] as IDictionary<string, object>).Values.ToArray()[1].Should().Be("123456789");
                    }
                }
            }
        }
    }
}
