using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Interfaces
{
    public interface ITrajectService
    {

        Task<List<Traject>> Add(Traject traject);
        Task<List<Traject>> Delete(Traject station);

        Task<List<Traject>> GetAll();

        Task SaveList(List<Traject> current);
    }
}
