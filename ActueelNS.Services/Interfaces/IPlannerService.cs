using System;
using ActueelNS.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActueelNS.Services.Interfaces
{
    public interface IPlannerService
    {
        Task<List<ReisMogelijkheid>> GetSearchResult(PlannerSearch search);

    }
}
