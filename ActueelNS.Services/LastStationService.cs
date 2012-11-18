using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;

namespace ActueelNS.Services
{
    public class LastStationService : ILastStationService
    {
        private readonly string fileName = "last_stations";

        StorageHelper<string[]> sh = new StorageHelper<string[]>(StorageType.Local);

        public IStationService StationService { get; set; }

        public List<Station> AllInMemmory { get; set; }

        public LastStationService(IStationService stationService)
        {
            StationService = stationService;
        }

        public async Task<List<Station>> Add(Station station1, Station station2, Station station3)
        {
            var current = await GetAll();

            if (current == null)
                current = new List<Station>();

            current.Reverse();

            current = AddSingleStation(station1, current);
            current = AddSingleStation(station2, current);
            current = AddSingleStation(station3, current);

            current.Reverse();
            current = current.Take(10).ToList();

            sh.SaveASync(current.Select(x => x.Code).ToArray(), fileName);

            AllInMemmory = current;

            return current;

        }

        private static List<Station> AddSingleStation(Station station1, List<Station> current)
        {
            if (station1 != null)
            {
                current = current.Where(x => x.Code != station1.Code).ToList();

                current.Add(station1);
            }

            return current;
        }

        public async Task<List<Station>> GetAll()
        {
            try
            {
                if (AllInMemmory != null)
                    return AllInMemmory;

                var load = await sh.LoadASync(fileName);

                if (load == null)
                    return null;

                var all = await StationService.GetStations("NL");

                var filtered = new List<Station>();

                foreach (var l in load)
                {
                    var s = all.Where(x => x.Code == l).FirstOrDefault();
                    if (s != null)
                        filtered.Add(s);
                }

                AllInMemmory = filtered.ToList();

                return AllInMemmory;

            }
            catch
            {
                return null;
            }
        }
    }
}
