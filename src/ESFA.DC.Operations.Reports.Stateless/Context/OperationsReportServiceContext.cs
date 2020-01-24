using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESFA.DC.ILR.Constants;
using ESFA.DC.JobContextManager.Model;
using ESFA.DC.Operations.Reports.Interface;

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

        public string Rule => _jobContextMessage.KeyValuePairs["Rule"].ToString();

        public int Year => int.Parse(_jobContextMessage.KeyValuePairs["Year"].ToString());

        public IEnumerable<string> Tasks => _jobContextMessage.Topics[_jobContextMessage.TopicPointer].Tasks.SelectMany(x => x.Tasks);
    }
}
