namespace ESFA.DC.Operations.Reports.Interface
{
    public interface IFileNameService
    {
        string Generate(IOperationsReportServiceContext operationsReportServiceContext, string reportName, OutputTypes outputType, bool includeDateTime = true, bool includeYearAndPeriod = true, bool includeJobId = false);
    }
}
