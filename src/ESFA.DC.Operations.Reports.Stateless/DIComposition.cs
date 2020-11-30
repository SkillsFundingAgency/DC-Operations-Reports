using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.FileService.Config;
using ESFA.DC.FundingClaims.Data;
using ESFA.DC.ILR1819.DataStore.EF;
using ESFA.DC.ILR1819.DataStore.EF.Interface;
using ESFA.DC.ILR1920.DataStore.EF;
using ESFA.DC.ILR1920.DataStore.EF.Interface;
using ESFA.DC.ILR2021.DataStore.EF;
using ESFA.DC.ILR2021.DataStore.EF.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Stateless.Config;
using ESFA.DC.Operations.Reports.Stateless.Modules;
using ESFA.DC.PIMS.EF;
using ESFA.DC.PIMS.EF.Interfaces;
using ESFA.DC.ServiceFabric.Common.Config;
using ESFA.DC.ServiceFabric.Common.Config.Interface;
using ESFA.DC.ServiceFabric.Common.Modules;
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

            containerBuilder.RegisterType<ILR2021_DataStoreEntities>().As<IIlr2021Context>();
            containerBuilder.RegisterType<ILR1920_DataStoreEntities>().As<IIlr1920RulebaseContext>();
            containerBuilder.RegisterType<ILR1819_DataStoreEntities>().As<IIlr1819RulebaseContext>();
            containerBuilder.RegisterType<PimsContext>().As<IPimsContext>();
            containerBuilder.RegisterType<FundingClaimsDataContext>().As<IFundingClaimsDataContext>().ExternallyOwned();
            containerBuilder.RegisterInstance(reportServiceConfiguration).As<IReportServiceConfiguration>();

            containerBuilder.Register(context =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ILR1819_DataStoreEntities>();
                    optionsBuilder.UseSqlServer(
                        reportServiceConfiguration.IlrDataStore1819ConnectionString,
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<ILR1819_DataStoreEntities>>()
                .SingleInstance();

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

            containerBuilder.Register(context =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ILR2021_DataStoreEntities>();
                    optionsBuilder.UseSqlServer(
                        reportServiceConfiguration.IlrDataStore2021ConnectionString,
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<ILR2021_DataStoreEntities>>()
                .SingleInstance();

            containerBuilder.Register(context =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<PimsContext>();
                    optionsBuilder.UseSqlServer(
                        reportServiceConfiguration.PimsDataConnectionString,
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<PimsContext>>()
                .SingleInstance();

            containerBuilder.Register(context =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<FundingClaimsDataContext>();
                optionsBuilder.UseSqlServer(
                    reportServiceConfiguration.FundingClaimsConnectionString,
                    options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                return optionsBuilder.Options;
            })
               .As<DbContextOptions<FundingClaimsDataContext>>()
               .SingleInstance();

            containerBuilder.RegisterModule(new StatelessBaseModule(statelessServiceConfiguration));
            containerBuilder.RegisterModule<SerializationModule>();
            containerBuilder.RegisterModule(new IOModule(azureStorageFileServiceConfiguration));
            containerBuilder.RegisterModule<ReportsServiceModule>();
            containerBuilder.RegisterModule<ReportsModule>();

            return containerBuilder;
        }
    }
}
