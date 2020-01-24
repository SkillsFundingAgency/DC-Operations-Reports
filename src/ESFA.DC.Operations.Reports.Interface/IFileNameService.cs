namespace ESFA.DC.Operations.Reports.Interface
{
    public interface IFileNameService
    {
        string Generate(IOperationsReportServiceContext mcaGlaReportServiceContext, string reportName, OutputTypes outputType, bool includeDateTime = true, bool includeYearAndPeriod = true);
    }
}
