using System;

namespace ESFA.DC.Operations.Reports.Interface
{
    public interface IReportServiceConfiguration
    {
        string IlrDataStore1819ConnectionString { get; }

        string IlrDataStore1920ConnectionString { get; }

        string IlrDataStore2021ConnectionString { get; }
    }
}
