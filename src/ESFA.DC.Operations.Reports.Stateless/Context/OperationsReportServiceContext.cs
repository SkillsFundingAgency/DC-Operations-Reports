using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.CollectionsManagement.Models;
using ESFA.DC.ILR.Constants;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.Operations.Reports.Interface;
using ESFA.DC.Operations.Reports.Reports.Constants;

namespace ESFA.DC.Operations.Reports.Stateless.Context
{
    public class OperationsReportServiceContext : IOperationsReportServiceContext
    {
        private readonly JobContextMessage _jobContextMessage;

        public OperationsReportServiceContext(JobContextMessage jobContextMessage)
        {
            _jobContextMessage = jobContextMessage;
        }

        public int JobId => (int)_jobContextMessage.JobId;

        public int Period => int.Parse(_jobContextMessage.KeyValuePairs[ILRContextKeys.ReturnPeriod].ToString());

        public string Container => _jobContextMessage.KeyValuePairs[ILRContextKeys.Container].ToString();
        
        public DateTime SubmissionDateTimeUtc => _jobContextMessage.SubmissionDateTimeUtc;

        public string Rule => _jobContextMessage.KeyValuePairs[MessageKeys.Rule].ToString();

        public int CollectionYear => int.Parse(_jobContextMessage.KeyValuePairs[ILRContextKeys.CollectionYear].ToString());

        public IEnumerable<string> Tasks => _jobContextMessage.Topics[_jobContextMessage.TopicPointer].Tasks.SelectMany(x => x.Tasks);

        public IEnumerable<ReturnPeriod> ILRPeriods => (IEnumerable<ReturnPeriod>)_jobContextMessage.KeyValuePairs[MessageKeys.ILRPeriods];

        public IEnumerable<ReturnPeriod> ILRPeriodsAdjustedTimes => GetReturnPeriodsWithAdjustedEndTimes((IEnumerable<ReturnPeriod>)_jobContextMessage.KeyValuePairs[MessageKeys.ILRPeriods]);

        public IEnumerable<ReturnPeriod> GetReturnPeriodsWithAdjustedEndTimes(IEnumerable<ReturnPeriod> returnPeriods)
        {
            foreach (ReturnPeriod period in returnPeriods.OrderBy(p => p.PeriodNumber))
            {
                if (period.PeriodNumber == 14)
                {
                    period.EndDateTimeUtc = period.EndDateTimeUtc.AddDays(14);
                }
                else if (returnPeriods.Any(p => p.PeriodNumber == period.PeriodNumber + 1))
                {
                    period.EndDateTimeUtc = returnPeriods.Single(p => p.PeriodNumber == period.PeriodNumber + 1).StartDateTimeUtc.AddSeconds(-1);
                }
            }

            return returnPeriods;
        }
    }
}
