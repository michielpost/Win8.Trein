using System.Collections.Generic;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Interfaces
{
    public interface IStationService
    {
        Task<List<Station>> GetStations(string country);

        Task<Station> GetStationByName(string name);
        Task<Station> GetStationByCode(string code);
    }
}
