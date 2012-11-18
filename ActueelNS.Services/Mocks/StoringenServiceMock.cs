using System;
using ActueelNS.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using ActueelNS.Services.Models;

namespace ActueelNS.Services.Mocks
{
    public class StoringenServiceMock : IStoringenService
    {
        public Task<List<Storing>> GetStoringen(string station)
        {
            throw new NotImplementedException();
        }


        public List<Werkzaamheden> GetWerkzaamheden()
        {
            throw new NotImplementedException();
        }
    }
}
