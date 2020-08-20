﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.CollectionsManagement.Models;
using ESFA.DC.Operations.Reports.Model;

namespace ESFA.DC.Operations.Reports.Interface.Providers
{
    public interface IILRProvidersReturningFirstTimePerDayProviderService
    {
        //Task<ICollection<ILRProvidersReturningFirstTimePerDayModel>> GetILRProvidersReturningFirstTimePerDay(CancellationToken cancellationToken);

        Task<ILRProvidersReturningFirstTimePerDayModel> GetILRProvidersReturningFirstTimePerDay(CancellationToken cancellationToken);
    }
}
