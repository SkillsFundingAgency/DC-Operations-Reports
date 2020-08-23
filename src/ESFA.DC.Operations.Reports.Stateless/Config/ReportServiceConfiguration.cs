using ESFA.DC.Operations.Reports.Interface;

namespace ESFA.DC.Operations.Reports.Stateless.Config
{
    public class ReportServiceConfiguration : IReportServiceConfiguration
    {
        public string IlrDataStore1819ConnectionString { get; set; }

        public string IlrDataStore1920ConnectionString { get; set; }
        
        public string IlrDataStore2021ConnectionString { get; set; }

        public string PimsDataConnectionString { get; set; }

        public string JobManagementConnectionString { get; set; }
    }
}
