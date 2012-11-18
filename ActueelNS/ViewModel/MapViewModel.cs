using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActueelNS.ViewModel
{
    public class MapViewModel : CustomViewModelBase
    {

        public GpsWatcherModel Gps
        {
            get
            {
                return SimpleIoc.Default.GetInstance<GpsWatcherModel>();
            }
        }

        private List<Station> _stations;


        public List<Station> Stations
        {
            get { return _stations; }
            set { _stations = value;
            RaisePropertyChanged(() => Stations);
            }
        }
        

         /// <summary>
        /// Initializes a new instance of the viewmodel class.
        /// </summary>
        public MapViewModel()
        {
            LoadData();
           
        }

        private async void LoadData()
        {
            Stations = await SimpleIoc.Default.GetInstance<IStationService>().GetStations("NL");
        }

    
    }
}
