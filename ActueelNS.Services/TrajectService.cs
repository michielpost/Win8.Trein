using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;

namespace ActueelNS.Services
{
    public class TrajectService : ITrajectService
    {
        private readonly string fileName = "traject";

        private Q42.WinRT.Storage.StorageHelper<List<Traject>> sh = new Q42.WinRT.Storage.StorageHelper<List<Traject>>(Q42.WinRT.Storage.StorageType.Local);


        //public IStationService StationService { get; set; }

        public List<Traject> AllInMemmory { get; set; }

        public TrajectService(IStationService stationService)
        {
            //StationService = stationService;
        }

        public async Task<List<Traject>> Add(Traject traject)
        {
            var current = await GetAll();

            if (current == null)
                current = new List<Traject>();

            current = AddSingleTraject(traject, current);

            SaveList(current);

            return current;

        }

        public async Task SaveList(List<Traject> current)
        {
            sh.SaveAsync(current, fileName);

            AllInMemmory = current;
        }

        private static List<Traject> AddSingleTraject(Traject t, List<Traject> current)
        {
            if (t != null)
            {
                current = current.Where(x => x.UniqueId != t.UniqueId).ToList();

                current.Add(t);
            }

            current = current.OrderBy(x => x.From.Name).ToList();

            return current;
        }

        public async Task<List<Traject>> Delete(Traject t)
        {

            var current = await GetAll();

            if (current == null)
                current = new List<Traject>();

            if (t != null)
            {
                current = current.Where(x => x.UniqueId != t.UniqueId).ToList();

                sh.SaveAsync(current, fileName);
            }

            AllInMemmory = current;

            return current;

        }

        public async Task<List<Traject>> GetAll()
        {
            try
            {
                if (AllInMemmory != null)
                    return AllInMemmory;

                var load = await sh.LoadAsync(fileName);

                if (load == null)
                    load = new List<Traject>();

                load = load.OrderBy(x => x.From.Name).ToList();

                AllInMemmory = load;

                return AllInMemmory;

            }
            catch
            {
                return new List<Traject>();
            }
        }
    }
}
