using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.ServiceFabric.Common.Config.Interface;
using ESFA.DC.ServiceFabric.Common.Modules;

namespace ESFA.DC.Operations.Reports.Stateless.Modules
{
    public class StatelessBaseModule : Module
    {
        private readonly IStatelessServiceConfiguration _statelessServiceConfiguration;

        public StatelessBaseModule(IStatelessServiceConfiguration statelessServiceConfiguration)
        {
            _statelessServiceConfiguration = statelessServiceConfiguration;
        }

        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule(new StatelessServiceModule(_statelessServiceConfiguration));
            containerBuilder.RegisterModule<SerializationModule>();
            containerBuilder.RegisterType<DateTimeProvider.DateTimeProvider>().As<IDateTimeProvider>();
            containerBuilder.RegisterType<MessageHandler>().As<IMessageHandler<JobContextMessage>>();
        }
    }
}
