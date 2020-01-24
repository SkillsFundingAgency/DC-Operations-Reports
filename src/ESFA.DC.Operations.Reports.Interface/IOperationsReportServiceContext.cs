using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Interface
{
    public interface IOperationsReportServiceContext
    {
        int JobId { get; }

        string Container { get; }

        int Period { get; }

        string Rule { get; }

        int Year { get; }

        DateTime SubmissionDateTimeUtc { get; }

        IEnumerable<string> Tasks { get; }
    }
}
