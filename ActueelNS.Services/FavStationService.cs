using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;

namespace ActueelNS.Services
{
    public class FavStationService : IFavStationService
    {
        private readonly string fileName = "fav_stations";

               StorageHelper<string[]> sh = new StorageHelper<string[]>(StorageType.Local);

        public IStationService StationService { get; set; }

        public List<Station> AllInMemmory { get; set; }

        public FavStationService(IStationService stationService)
        {
            StationService = stationService;
        }

        public async Task<List<Station>> Add(Station station)
        {
            var current = await GetAll();

            if (current == null)
                current = new List<Station>();

            current = AddSingleStation(station, current);

            sh.SaveASync(current.Select(x => x.Code).ToArray(), fileName);

            AllInMemmory = current;

            return current;

        }

        public async Task SaveList(List<Station> current)
        {
            sh.SaveASync(current.Select(x => x.Code).ToArray(), fileName);

            AllInMemmory = current;
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

        public async Task<List<Station>> Delete(Station station)
        {

            var current = await GetAll();

            if (current == null)
                current = new List<Station>();

            if (station != null)
            {
                current = current.Where(x => x.Code != station.Code).ToList();

                sh.SaveASync(current.Select(x => x.Code).ToArray(), fileName);
            }

            AllInMemmory = current;

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
                    return new List<Station>();

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
                return new List<Station>();
            }
        }
    }
}
