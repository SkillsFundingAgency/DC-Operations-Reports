using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Interface.Providers;
using ESFA.DC.Operations.Reports.Model;
using ESFA.DC.Operations.Reports.Reports.Constants;
using ESFA.DC.Operations.Reports.Reports.Extensions;

namespace ESFA.DC.Operations.Reports.Reports.ValidationRuleDetailReport
{
    public class FundingClaimsProviderSubmissions1920ReportModelBuilder : IModelBuilder<FundingClaimsSubmissionsModel>
    {
        private readonly IFundingClaimsProviderService _fundingClaimsProviderService;

        public FundingClaimsProviderSubmissions1920ReportModelBuilder(
            IFundingClaimsProviderService fundingClaimsProviderService)
        {
            _fundingClaimsProviderService = fundingClaimsProviderService;
        }

        public async Task<FundingClaimsSubmissionsModel> Build(IOperationsReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
           return new FundingClaimsSubmissionsModel();
        }
    }
}
