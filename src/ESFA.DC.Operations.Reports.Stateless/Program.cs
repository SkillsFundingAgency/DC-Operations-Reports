using System;
using System.Diagnostics;
using System.Fabric;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.ServiceFabric;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.JobContextManager.Model.Interface;
using ESFA.DC.Operations.Reports.Stateless.Config;
using ESFA.DC.ServiceFabric.Common.Config;
using ESFA.DC.ServiceFabric.Common.Config.Interface;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ESFA.DC.Operations.Reports.Stateless
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                IServiceFabricConfigurationService serviceFabricConfigurationService = new ServiceFabricConfigurationService();
                
                // License Aspose.Cells
                SoftwareLicenceSection softwareLicenceSection = serviceFabricConfigurationService.GetConfigSectionAs<SoftwareLicenceSection>(nameof(SoftwareLicenceSection));
                if (!string.IsNullOrEmpty(softwareLicenceSection.AsposeLicence))
                {
                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(softwareLicenceSection.AsposeLicence.Replace("&lt;", "<").Replace("&gt;", ">"))))
                    {
                        new Aspose.Cells.License().SetLicense(ms);
                    }
                }


                // Setup Autofac
                ContainerBuilder builder = DIComposition.BuildContainer();

                // Register the Autofac magic for Service Fabric support.
                builder.RegisterServiceFabricSupport();

                // Register the stateless service.
                builder.RegisterStatelessService<ServiceFabric.Common.Stateless>("ESFA.DC.Operations.Reports.StatelessType");

                using (var container = builder.Build())
                {
                    var jobContextMessage = container.Resolve<IMessageHandler<JobContextMessage>>();
                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ServiceFabric.Common.Stateless).Name);

                    // Prevents this host process from terminating so services keep running.
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
