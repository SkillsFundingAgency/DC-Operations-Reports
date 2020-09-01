using System.Collections.Generic;
using Autofac;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.ILRFileSubmissionPerDayReport;
using ESFA.DC.Operations.Reports.Reports.ILRFileSubmissionsPerDayReport;
using ESFA.DC.Operations.Reports.Reports.ILRProvidersReturningFirstTimePerDayReport;
using ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport;
using ESFA.DC.Operations.Reports.Service.Providers;


namespace ESFA.DC.Operations.Reports.Stateless.Modules
{
    public class ReportsModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ValidationRuleDetailReport>().As<IReport>();
            containerBuilder.RegisterType<ILRProvidersReturningFirstTimePerDayReport>().As<IReport>();
            containerBuilder.RegisterType<ILRFileSubmissionPerDayReport>().As<IReport>();
            containerBuilder.RegisterType<ValidationRuleDetailReportModelBuilder>().As<IModelBuilder<IEnumerable<ValidationRuleDetail>>>();
            containerBuilder.RegisterType<ILRProvidersReturningFirstTimePerDayReportModelBuilder>().As<IModelBuilder<ILRProvidersReturningFirstTimePerDayModel>>();
            containerBuilder.RegisterType<ILRFileSubmissionPerDayReportModelBuilder>().As<IModelBuilder<ILRFileSubmissionsPerDayModel>>();
        }
    }
}
