using Autofac;
using ESFA.DC.CsvService;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Operations.Reports.Service;
using ESFA.DC.Operations.Reports.Service.Providers;

namespace ESFA.DC.Operations.Reports.Stateless.Modules
{
    public class ReportsServiceModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ReportGenerationService>().As<IReportGenerationService>();
            containerBuilder.RegisterType<FileNameService>().As<IFileNameService>();
            containerBuilder.RegisterType<CsvFileService>().As<ICsvFileService>();
            containerBuilder.RegisterType<ValidationRuleDetails1819ProviderService>().Keyed<IValidationRuleDetailsProviderService>(ILRYears.Year1819);
            containerBuilder.RegisterType<ValidationRuleDetails1920ProviderService>().Keyed<IValidationRuleDetailsProviderService>(ILRYears.Year1920);
            containerBuilder.RegisterType<ValidationRuleDetails2021ProviderService>().Keyed<IValidationRuleDetailsProviderService>(ILRYears.Year2021);

            containerBuilder.RegisterType<ILRProvidersReturningFirstTimePerDayProviderService>().As<IILRProvidersReturningFirstTimePerDayProviderService>();

            containerBuilder.RegisterType<OrgProviderService>().As<IOrgProviderService>();
        }
    }
}
