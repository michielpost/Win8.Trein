using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Interfaces
{
    public interface ILastStationService
    {
        Task<List<Station>> Add(Station station1, Station station2, Station station3);

        Task<List<Station>> GetAll();
    }
}
