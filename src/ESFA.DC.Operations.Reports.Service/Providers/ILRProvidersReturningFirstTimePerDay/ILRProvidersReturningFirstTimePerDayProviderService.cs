using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CollectionsManagement.Models;
using ESFA.DC.ILR1920.DataStore.EF.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Service.Providers.Abstract;

namespace ESFA.DC.Operations.Reports.Service.Providers
{
    public class ILRProvidersReturningFirstTimePerDayProviderService : IILRProvidersReturningFirstTimePerDayProviderService
    {
        private readonly Func<IIlr1920RulebaseContext> _ilrContextFactory;

        public ILRProvidersReturningFirstTimePerDayProviderService(
            ILogger logger,
            Func<IIlr1920RulebaseContext> ilrContextFactory)
        {
            _ilrContextFactory = ilrContextFactory;
        }

        public async Task<ILRProvidersReturningFirstTimePerDayModel> GetILRProvidersReturningFirstTimePerDay(CancellationToken cancellationToken)
        {
            var model = new ILRProvidersReturningFirstTimePerDayModel();
            var ilrProvidersReturningFirstTimePerDays = new List<ILRProvidersReturningFirstTimePerDay>()
            {
                new ILRProvidersReturningFirstTimePerDay()
                {
                    Days = -25,
                    Submissions = 100
                },
                new ILRProvidersReturningFirstTimePerDay()
                {
                    Days = -24,
                    Submissions = 90
                },
                new ILRProvidersReturningFirstTimePerDay()
                {
                    Days = -23,
                    Submissions = 80
                },
                new ILRProvidersReturningFirstTimePerDay()
                {
                    Days = -22,
                    Submissions = 70
                },
                new ILRProvidersReturningFirstTimePerDay()
                {
                    Days = -21,
                    Submissions = 150
                },
            };

            model.IlrProvidersReturningFirstTimePerDaysList = ilrProvidersReturningFirstTimePerDays;
            return model;
        }
    }
}
