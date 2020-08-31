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
    public class ValidationRuleDetails1920ProviderService : AbstractValidationRuleDetailsProviderService, IValidationRuleDetailsProviderService
    {
        private readonly Func<IIlr1920RulebaseContext> _ilrContextFactory;

        public ValidationRuleDetails1920ProviderService(
            ILogger logger,
            Func<IIlr1920RulebaseContext> ilrContextFactory)
        {
            _ilrContextFactory = ilrContextFactory;
        }

        public async Task<ICollection<ValidationRuleDetail>> GetValidationRuleDetails(string rule, IEnumerable<ReturnPeriod> returnPeriods,  CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var validationRuleDetails = new List<ValidationRuleDetail>();

            using (var ilrContext = _ilrContextFactory())
            {
                validationRuleDetails = (from ve in ilrContext.ValidationErrors
                    join fd in ilrContext.FileDetails
                        on ve.Source equals fd.Filename
                    where ve.RuleName == rule
                    select new { ve.RuleName, ve.UKPRN, fd.SubmittedTime, ve.Severity } into x
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
    }
}
