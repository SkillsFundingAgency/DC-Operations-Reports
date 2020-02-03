using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Constants;

namespace ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport
{
    public class ValidationRuleDetailReportModelBuilder : IModelBuilder<IEnumerable<ValidationRuleDetail>>
    {
        private readonly IIndex<ILRYears, IValidationRuleDetailsProviderService> _validationRulesProviderServices;
        private readonly IOrgProviderService _orgProviderService;

        public ValidationRuleDetailReportModelBuilder(
            IIndex<ILRYears, IValidationRuleDetailsProviderService> validationRulesProviderServices,
            IOrgProviderService orgProviderService)
        {
            _validationRulesProviderServices = validationRulesProviderServices;
            _orgProviderService = orgProviderService;
        }

        public async Task<IEnumerable<ValidationRuleDetail>> Build(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var ilrPeriodsAdjustedTimes = reportServiceContext.ILRPeriodsAdjustedTimes;
            var rule = reportServiceContext.Rule;
            var validationRuleDetailsProviderService = _validationRulesProviderServices[(ILRYears)reportServiceContext.CollectionYear];

            var validationRuleDetails = await validationRuleDetailsProviderService.GetValidationRuleDetails(rule, ilrPeriodsAdjustedTimes, cancellationToken);
            var ukprns = validationRuleDetails.Where(x => x.UkPrn != null).Select(x => (long)x.UkPrn);

            IEnumerable<OrgModel> orgDetails = await _orgProviderService.GetOrgDetailsForUKPRNsAsync(ukprns.Distinct().ToList(), CancellationToken.None);
            PopulateModelsWithOrgDetails(validationRuleDetails, orgDetails);

            return validationRuleDetails;
        }

        public static void PopulateModelsWithOrgDetails(IEnumerable<ValidationRuleDetail> validationRuleDetails, IEnumerable<OrgModel> orgDetails)
        {
            foreach (var validationRuleDetail in validationRuleDetails)
            {
                validationRuleDetail.ProviderName = orgDetails.SingleOrDefault(p => p.Ukprn == validationRuleDetail.UkPrn)?.Name;
            }
        }
    }
}
