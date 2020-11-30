using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESFA.DC.CollectionsManagement.Models;

namespace ESFA.DC.Operations.Reports.Service.Providers.Abstract
{
    public abstract class AbstractValidationRuleDetailsProviderService
    {
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
