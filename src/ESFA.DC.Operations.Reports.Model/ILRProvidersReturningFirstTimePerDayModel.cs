using System.Collections.Generic;

namespace ESFA.DC.Operations.Reports.Model
{
    public class ILRProvidersReturningFirstTimePerDayModel
    {
        public string Period { get; set; }

        public List<ILRProvidersReturningFirstTimePerDay> IlrProvidersReturningFirstTimePerDaysList { get; set; }
    }
}
