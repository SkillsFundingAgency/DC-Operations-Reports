using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ESFA.DC.JobContextManager.Interface;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.Logging;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Stateless.Context;

namespace ESFA.DC.Operations.Reports.Stateless
{
    public class MessageHandler : IMessageHandler<JobContextMessage>
    {
        private readonly IReportGenerationService _reportGenerationService;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly ILogger _logger;

        public MessageHandler(
            IReportGenerationService reportGenerationService,
            ILifetimeScope lifetimeScope,
            ILogger logger)
        {
            _reportGenerationService = reportGenerationService;
            _lifetimeScope = lifetimeScope;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(JobContextMessage message, System.Threading.CancellationToken cancellationToken)
        {
            _logger.LogInfo("Entered Message Handler");

            var reportServiceContext = new OperationsReportServiceContext(message);

            using (var childLifetimeScope = _lifetimeScope.BeginLifetimeScope())
            {
                var executionContext = (ExecutionContext)childLifetimeScope.Resolve<IExecutionContext>();
                executionContext.JobId = message.JobId.ToString();

                try
                {
                    await _reportGenerationService.GenerateAsync(reportServiceContext, cancellationToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError("Reference Data Message Handler Exception", exception);
                    throw;
                }

                return true;
            }
        }
    }
}
