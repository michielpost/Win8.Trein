using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActueelNS.Services.Mocks
{
    public class StationServiceMock : IStationService
    {

        public async Task<List<Station>> GetStations(string country)
        {
            List<Station> stationList = new List<Station>();

            var d = new Station()
            {
                Code = "Dt",
                Name = "Delft"
            };
            d.SetDistance(1000);

            stationList.Add(d);

            stationList.Add(new Station()
            {
                Code = "Dt",
                Name = "Amsterdam"
            });

            stationList.Add(new Station()
            {
                Code = "Dt",
                Name = "Rijswijk"
            });

            return new List<Station>(stationList);
        }

        public async Task<IList<Station>> GetMyStations()
        {
            List<Station> stationList = new List<Station>();

            stationList.Add(new Station()
            {
                Code = "Dt",
                Name = "Delft"
            });

            stationList.Add(new Station()
            {
                Code = "Dt",
                Name = "Amsterdam"
            });

            stationList.Add(new Station()
            {
                Code = "Dt",
                Name = "Rijswijk"
            });

            return stationList;

        }

        Task<Station> IStationService.GetStationByName(string name)
        {
            throw new System.NotImplementedException();
        }

        Task<Station> IStationService.GetStationByCode(string name)
        {
            throw new System.NotImplementedException();
        }


       
    }
}
