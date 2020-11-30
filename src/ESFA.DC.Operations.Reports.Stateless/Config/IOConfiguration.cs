using ESFA.DC.IO.AzureStorage.Config.Interfaces;

namespace ESFA.DC.Operations.Reports.Stateless.Config
{
    public class IOConfiguration : IAzureStorageKeyValuePersistenceServiceConfig
    {
        public string ConnectionString { get; set; }

        public string ContainerName { get; set; }
    }
}
