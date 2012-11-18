using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;
using ActueelNS.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Windows.ApplicationModel.Resources;
using Windows.UI.ApplicationSettings;

namespace ActueelNS.ViewModel
{
    
    public enum InputType
    {
        None,
        Van,
        Naar,
        Via
    }

    public class PlannerViewModel : CustomViewModelBase
    {
        private ResourceLoader _resourceLoader = new ResourceLoader("Resources");

      

        private ObservableCollection<Station> _allStations = new ObservableCollection<Station>();

        public ObservableCollection<Station> AllStations
        {
            get { return _allStations; }
            set
            {
                _allStations = value;
                RaisePropertyChanged(() => AllStations);
            }
        }

        private List<string> _allNames;

        public List<string> AllNames
        {
            get { return _allNames; }
            set { _allNames = value;
            RaisePropertyChanged(() => AllNames);
            }
        }
        

        

        private Station _vanStation;

        public Station VanStation
        {
            get {

                return _vanStation; }
            set
            {
                _vanStation = value;
                RaisePropertyChanged(() => VanStation);

                if (value != null)
                    ShowFromError = false;
            }
        }

        private Station _naarStation;

        public Station NaarStation
        {
            get
            {
                return _naarStation;
            }
            set
            {
                _naarStation = value;
                RaisePropertyChanged(() => NaarStation);

                if (value != null)
                    ShowToError = false;
            }
        }

        private Station _viaStation;

        public Station ViaStation
        {
            get
            {
                return _viaStation;
            }
            set
            {
                _viaStation = value;
                RaisePropertyChanged(() => ViaStation);
                RaisePropertyChanged(() => IsViaVisible);
            }
        }

        private bool _isViaVisible;

        public bool IsViaVisible
        {
            get
            {

                if (ViaStation != null)
                    return true;
                else
                    return _isViaVisible;
            }
            set
            {
                _isViaVisible = value;
                RaisePropertyChanged(() => IsViaVisible);
                RaisePropertyChanged("IsViaCustomVisible");
            }
        }

        private DateTime? _planDateTime;

        public DateTime? PlanDateTime
        {
            get { return _planDateTime; }
            set
            {
                _planDateTime = value;
                RaisePropertyChanged(() => PlanDateTime);
            }
        }

        private bool _showHslMessage;

        public bool ShowHslMessage
        {
            get { return _showHslMessage; }
            set { _showHslMessage = value;
            RaisePropertyChanged(() => ShowHslMessage);
            }
        }
        



        public ILastStationService LastStationService { get; set; }
        public IStationService StationService { get; set; }
        public ISettingService SettingService { get; set; }
       


        private string _type;

        public string Type
        {
            get { return _type; }
            set {
                if (value != null)
                {
                    _type = value;
                    RaisePropertyChanged(() => Type);
                }
            }
        }


        private bool _showFromError;

        public bool ShowFromError
        {
            get { return _showFromError; }
            set { _showFromError = value;
            RaisePropertyChanged(() => ShowFromError);
            }
        }

        private bool _showToError;

        public bool ShowToError
        {
            get { return _showToError; }
            set { _showToError = value;
            RaisePropertyChanged(() => ShowToError);
            }
        }

        private bool _showErrorDifferent;

        public bool ShowErrorDifferent
        {
            get { return _showErrorDifferent; }
            set { _showErrorDifferent = value;
            RaisePropertyChanged(() => ShowErrorDifferent);
            }
        }




        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand NoHslMessageCommand { get; private set; }
        public RelayCommand YesHslMessageCommand { get; private set; }

        public PlannerViewModel(ILastStationService lastStationService, IStationService stationService, ISettingService settingService)
        {
            LastStationService = lastStationService;
            StationService = stationService;
            SettingService = settingService;

            Type = "vertrek";

            

            Messenger.Default.Register<Station>(this, "SetGpsStation", SetGpsStation);


            SearchCommand = new RelayCommand(() => DoSearch());
            NoHslMessageCommand = new RelayCommand(() => NoHslMessageAction());
            YesHslMessageCommand = new RelayCommand(() => YesHslMessageAction());

            LoadAllStations();

            ShowHslMessage = settingService.GetShowHslMessage();

        }

        private void NoHslMessageAction()
        {
            ShowHslMessage = false;
            SettingService.SaveShowHslMessage(false);
        }

        private void YesHslMessageAction()
        {
            SettingService.SaveShowHslMessage(false);
            SettingsPane.Show();
        }

        private async void LoadAllStations()
        {
            var list = await StationService.GetStations("NL");

            List<string> allNames = new List<string>();
            foreach (var item in list)
            {
                allNames.Add(item.Name);

                foreach (var se in item.NamesExtra)
                {
                    allNames.Add(se);

                }
            }

            AllNames = allNames;
            AllStations = new ObservableCollection<Station>(list);
        }

        public void SetGpsStation(Station input)
        {
            if (this.VanStation == null)
                VanStation = input;
        }

        
        

        private async void DoSearch()
        {
            if (!PlanDateTime.HasValue || PlanDateTime.Value == DateTime.MinValue)
                PlanDateTime = DateTime.Now;
          
            if (string.IsNullOrEmpty(Type))
                Type = "vertrek";

            bool isHsl = SettingService.GetShowHsl();
            bool isYearCard = SettingService.GetOvCard();

            //Create planner object with GUID
            PlannerSearch search = new PlannerSearch()
            {
                Id = Guid.NewGuid(),
                SearchDateTime = DateTime.Now,
                VanStation = VanStation,
                NaarStation = NaarStation,
                ViaStation = ViaStation,
                IsHogesnelheid = isHsl,
                IsYearCard = isYearCard,
                Type = Type,
                DateTime = PlanDateTime.Value
            };

            if (search.VanStation == null)
                ShowFromError = true;

            if (search.NaarStation == null)
                ShowToError = true;

            if (search.VanStation != null
                && search.NaarStation != null)
            {
               //TODO Save default settings
                if (search.VanStation == search.NaarStation)
                {
                    this.ShowErrorDifferent = true;
                }
                else
                {

                    //Save last used
                    var list = await LastStationService.Add(VanStation, NaarStation, ViaStation);
                    //LastStations = new ObservableCollection<Station>(list);

                    //LoadLastStations();

                    //Navigate to new page 
                    App.RootFrame.Navigate(typeof(TravelAdvicePage), search);
                }
            }


        }


        internal void SetDateNow()
        {
            PlanDateTime = DateTime.Now;

        }


    

      

        internal void FinishAutoComplete(InputType type, string value)
        {

            var selectedStation = GetStationByName(value);


            if (type == InputType.Van)
            {
                this.VanStation = selectedStation;
            }
            else if (type == InputType.Naar)
            {
                this.NaarStation = selectedStation;
              
            }
            else if (type == InputType.Via)
            {
                this.ViaStation = selectedStation;

            }


        }

        public Station GetStationByName(string value)
        {
            var selectedStation = AllStations.Where(x => x.Name.ToLower() == value.ToLower()).FirstOrDefault();
            if (selectedStation == null)
            {
                selectedStation = AllStations.Where(x => x.NamesExtra.Select(y => y.ToLower()).Contains(value.ToLower())).FirstOrDefault();
            }
            return selectedStation;
        }



        internal void Initialize()
        {
            //this.PlanDateTime = null;

            this.ShowErrorDifferent = false;
            this.ShowToError = false;
            this.ShowFromError = false;

        }

       

        public void WisselVanNaar()
        {
            var tempNaar = this._naarStation;

            this.NaarStation = this._vanStation;
            this.VanStation = tempNaar;

            if (VanStation != null)
                Messenger.Default.Send(VanStation.Name, "setVan");
            if (NaarStation != null)
                Messenger.Default.Send(NaarStation.Name, "setNaar");
        }
    }
}
