using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport;


namespace ESFA.DC.Operations.Reports.Stateless.Modules
{
    public class ReportsModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<ValidationRuleDetailReport>().As<IReport>();
        }
    }
}
