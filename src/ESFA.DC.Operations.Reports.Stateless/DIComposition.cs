using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.FileService.Config;
using ESFA.DC.ILR1920.DataStore.EF;
using ESFA.DC.ILR1920.DataStore.EF.Interface;
using ESFA.DC.Operations.Reports.Stateless.Config;
using ESFA.DC.Operations.Reports.Stateless.Modules;
using ESFA.DC.ServiceFabric.Common.Config;
using ESFA.DC.ServiceFabric.Common.Config.Interface;
using Microsoft.EntityFrameworkCore;

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

            containerBuilder.RegisterType<ILR1920_DataStoreEntities>().As<IIlr1920RulebaseContext>();
            containerBuilder.Register(context =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ILR1920_DataStoreEntities>();
                    optionsBuilder.UseSqlServer(
                        reportServiceConfiguration.IlrDataStore1920ConnectionString,
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<ILR1920_DataStoreEntities>>()
                .SingleInstance();

            containerBuilder.RegisterModule(new StatelessBaseModule(statelessServiceConfiguration));
            containerBuilder.RegisterModule(new IOModule(azureStorageFileServiceConfiguration));
            containerBuilder.RegisterModule<ReportsServiceModule>();
            containerBuilder.RegisterModule<ReportsModule>();

            return containerBuilder;
        }
    }
}
