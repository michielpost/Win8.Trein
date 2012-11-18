using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;
using GalaSoft.MvvmLight;
using Windows.ApplicationModel.Resources;

namespace ActueelNS.ViewModel
{
    public class SearchResultsViewModel : CustomViewModelBase
    {
        private readonly IStationService _stationService;
        private ResourceLoader _resourceLoader = new ResourceLoader("Resources");


        private ObservableCollection<Station> _suggestStations = new ObservableCollection<Station>();

        public ObservableCollection<Station> SuggestStations
        {
            get { return _suggestStations; }
            set
            {
                _suggestStations = value;
                RaisePropertyChanged(() => SuggestStations);
            }
        }

        public string PageTitle
        {
            get
            {
                if (string.IsNullOrEmpty(QueryText))
                {
                   return _resourceLoader.GetString("SearchResultsViewModelNoInput");
                }
                else
                {
                    return string.Format(_resourceLoader.GetString("SearchResultsViewModelSearchFor"), QueryText);
                }
            }
        }

        private string _queryText;

        public string QueryText
        {
            get { return _queryText; }
            set { _queryText = value;
            RaisePropertyChanged(() => QueryText);
            RaisePropertyChanged(() => PageTitle);
            }
        }
        

        public SearchResultsViewModel(IStationService stationService)
        {
            _stationService = stationService;

            if (ViewModelBase.IsInDesignModeStatic)
            {
                List<Station> stationDesignList = new List<Station>();
                stationDesignList.Add(new Station() { Name = "Amsterdam Centraal" });
                stationDesignList.Add(new Station() { Name = "Delft" });

                foreach (var l in stationDesignList)
                    SuggestStations.Add(l);
            }


        }

        public async void SearchStation(string p)
        {
            QueryText = p;

            if (string.IsNullOrEmpty(p))
                SuggestStations.Clear();
            else
            {
                p = p.ToLower();

                //var newStations = await TaskEx.Run<List<Station>>(() =>
                //    {

                var stations = (await _stationService.GetStations("NL")).Where(x => x.Name.ToLower().StartsWith(p)).Take(15);

                if (stations.Count() < 15)
                {
                    var extraStations = (await _stationService.GetStations("NL")).Where(x => x.StartsWith(p)).Take(15 - stations.Count());

                    stations = stations.Union(extraStations);
                }

                //return stations.ToList();


                // });

                SuggestStations.Clear();

                //newStations.ForEach(x => StationList.Add(x));

                foreach (var s in stations)
                    SuggestStations.Add(s);

                //StationList = stations;
            }
        }


    }
}
