using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Model
{
    public class ILRProvidersReturningFirstTimePerDay
    {
        public int DaysToClose { get; set; }

        public DateTime DateTimeCreatedUTC { get; set; }

        public int NumberOfSubmissions { get; set; }
    }
}
