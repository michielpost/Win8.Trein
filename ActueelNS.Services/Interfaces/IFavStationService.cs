using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Interfaces
{
    public interface IFavStationService
    {

        Task<List<Station>> Add(Station station);
        Task<List<Station>> Delete(Station station);

        Task<List<Station>> GetAll();

        Task SaveList(List<Station> list);
    }
}
