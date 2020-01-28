using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.CsvService;
using ESFA.DC.CsvService.Interface;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
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
            containerBuilder.RegisterType<ValidationRuleDetails1920ProviderService>().As<IValidationRuleDetailsProviderService>();
        }
    }
}
