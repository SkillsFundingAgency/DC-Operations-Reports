using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CollectionsManagement.Models;
using ESFA.DC.ILR1819.DataStore.EF.Interface;
using ESFA.DC.ILR1920.DataStore.EF;
using ESFA.DC.ILR1920.DataStore.EF.Interface;
using ESFA.DC.Logging.Interfaces;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Service.Providers
{
    public class ValidationRuleDetails1819ProviderService : IValidationRuleDetailsProviderService
    {
        private readonly Func<IIlr1819RulebaseContext> _ilrContextFactory;

        public ValidationRuleDetails1819ProviderService(
            ILogger logger,
            Func<IIlr1819RulebaseContext> ilrContextFactory)
        {
            _ilrContextFactory = ilrContextFactory;
        }

        public async Task<ICollection<ValidationRuleDetail>> ProvideAsync(string rule, IEnumerable<ReturnPeriod> returnPeriods,  CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var validationRuleDetails = new List<ValidationRuleDetail>();

            using (var ilrContext = _ilrContextFactory())
            {
                validationRuleDetails = (from ve in ilrContext.ValidationErrors
                    join fd in ilrContext.FileDetails
                        on ve.Source equals fd.Filename
                    where ve.RuleName == rule
                    select new { ve.RuleName, ve.UKPRN, fd.SubmittedTime, ve.Severity} into x
                    group x by new { x.RuleName, x.UKPRN, x.SubmittedTime, x.Severity } into g
                    orderby g.Key.SubmittedTime descending
                    select new ValidationRuleDetail()
                    {
                        UkPrn = g.Key.UKPRN,
                        SubmissionDate = g.Key.SubmittedTime.GetValueOrDefault(),
                        Errors = g.Count(x => x.Severity.Equals("E", StringComparison.OrdinalIgnoreCase)),
                        Warnings = g.Count(x => x.Severity.Equals("W", StringComparison.OrdinalIgnoreCase)),
                        ReturnPeriod = $"R{GetPeriodReturn(g.Key.SubmittedTime.GetValueOrDefault(), returnPeriods):D2}"
                    }).ToList();
            }

            return validationRuleDetails;
        }

        public int GetPeriodReturn(DateTime? submittedDateTime, IEnumerable<ReturnPeriod> returnPeriods)
        {
            return !submittedDateTime.HasValue
                ? 0
                : returnPeriods
                      .SingleOrDefault(x =>
                          submittedDateTime >= x.StartDateTimeUtc &&
                          submittedDateTime <= x.EndDateTimeUtc)
                      ?.PeriodNumber ?? 99;
        }
    }
}
