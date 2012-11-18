using ActueelNS.Services.Models;
using System.Threading.Tasks;

namespace ActueelNS.Services.Interfaces
{
    public interface IPrijsService
    {
        Task<ReisPrijs> GetPrijs(PlannerSearch search);
    }
}
