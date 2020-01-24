using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.FileService.Config;
using ESFA.DC.Operations.Reports.Stateless.Config;
using ESFA.DC.Operations.Reports.Stateless.Modules;
using ESFA.DC.ServiceFabric.Common.Config;
using ESFA.DC.ServiceFabric.Common.Config.Interface;

namespace ESFA.DC.Operations.Reports.Stateless
{
    public static class DIComposition
    {
        public static ContainerBuilder BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();

            IServiceFabricConfigurationService serviceFabricConfigurationService = new ServiceFabricConfigurationService();

            var statelessServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAsStatelessServiceConfiguration();
            var reportServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<ReportServiceConfiguration>("ReportServiceConfiguration");
            var azureStorageFileServiceConfiguration = serviceFabricConfigurationService.GetConfigSectionAs<AzureStorageFileServiceConfiguration>("AzureStorageFileServiceConfiguration");

            containerBuilder.RegisterModule(new StatelessBaseModule(statelessServiceConfiguration));
            containerBuilder.RegisterModule(new IOModule(azureStorageFileServiceConfiguration));
            containerBuilder.RegisterModule<ReportsServiceModule>();
            containerBuilder.RegisterModule<ReportsModule>();

            return containerBuilder;
        }
    }
}
