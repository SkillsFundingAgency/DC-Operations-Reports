using System;
using System.Collections.Generic;
using ESFA.DC.CollectionsManagement.Models;

namespace ESFA.DC.Operations.Reports.Interface
{
    public interface IOperationsReportServiceContext
    {
        int JobId { get; }

        string Container { get; }

        int Period { get; }

        string Rule { get; }

        int CollectionYear { get; }

        DateTime SubmissionDateTimeUtc { get; }

        IEnumerable<string> Tasks { get; }

        IEnumerable<ReturnPeriod> ILRPeriods { get; }

        IEnumerable<ReturnPeriod> ILRPeriodsAdjustedTimes { get; }

        int SelectedCollectionYear { get; }

        IEnumerable<ReturnPeriod> SelectedILRPeriods { get; }

        IEnumerable<ReturnPeriod> SelectedILRPeriodsAdjustedTimes { get; }

        string ReturnPeriodName { get; }
    }
}
