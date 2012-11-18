using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;

namespace ActueelNS.Services
{
    public class SearchHistoryService : ISearchHistoryService
    {
        private readonly string fileName = "sh";

        private Q42.WinRT.Storage.StorageHelper<List<SearchHistory>> sh = new Q42.WinRT.Storage.StorageHelper<List<SearchHistory>>(Q42.WinRT.Storage.StorageType.Local);


        //public IStationService StationService { get; set; }

        public List<SearchHistory> AllInMemmory { get; set; }

        public SearchHistoryService(IStationService stationService)
        {
            //StationService = stationService;
        }

        public async Task<List<SearchHistory>> Add(SearchHistory traject)
        {
            var current = await GetAll();

            if (current == null)
                current = new List<SearchHistory>();

            current = AddSingleTraject(traject, current);

            await sh.SaveAsync(current, fileName);

            AllInMemmory = current;

            return current;

        }

        public async Task SaveList(List<SearchHistory> current)
        {
            sh.SaveAsync(current, fileName);

            AllInMemmory = current;
        }

        private static List<SearchHistory> AddSingleTraject(SearchHistory t, List<SearchHistory> current)
        {
            if (t != null && current != null && t.Reismogelijkheden.Count > 0)
            {
                current = current.Where(x => x.UniqueId != t.UniqueId).ToList();

                current.Insert(0,t);
            }

            return current.Take(10).ToList();
        }

        public async Task<List<SearchHistory>> Delete(SearchHistory t)
        {

            var current = await GetAll();

            if (current == null)
                current = new List<SearchHistory>();

            if (t != null)
            {
                current = current.Where(x => x.UniqueId != t.UniqueId).ToList();

                sh.SaveAsync(current, fileName);
            }

            AllInMemmory = current;

            return current;

        }

        public async Task<List<SearchHistory>> GetAll()
        {
            try
            {
                if (AllInMemmory != null)
                    return AllInMemmory;

                var load = await sh.LoadAsync(fileName);

                if (load == null)
                    return new List<SearchHistory>();

                AllInMemmory = load;

                return AllInMemmory;

            }
            catch
            {
                return new List<SearchHistory>();
            }
        }


        public async Task Clear()
        {
            var current = new List<SearchHistory>();

            await sh.SaveAsync(current, fileName);

            AllInMemmory = current;
        }
    }
}
