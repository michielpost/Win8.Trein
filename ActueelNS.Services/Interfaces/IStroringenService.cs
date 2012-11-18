using System.Threading.Tasks;
using ActueelNS.Services.Models;
using System.Collections.Generic;

namespace ActueelNS.Services.Interfaces
{
    public interface IStoringenService
    {
        Task<List<Storing>> GetStoringen(string station);

        List<Werkzaamheden> GetWerkzaamheden();
    }
}
