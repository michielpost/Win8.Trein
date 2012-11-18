using ActueelNS.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActueelNS.Services.Interfaces
{
    public interface IVertrektijdenService
    {
        Task<List<Vertrektijd>> GetVertrektijden(string station);
    }
}
