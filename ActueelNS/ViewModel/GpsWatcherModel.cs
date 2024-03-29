﻿using System;
using System.Net;
using System.Windows;
using System.Collections.ObjectModel;
using ActueelNS.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using System.Linq;
using ActueelNS.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight.Threading;
using GalaSoft.MvvmLight.Messaging;

namespace ActueelNS.ViewModel
{
    public class GpsWatcherModel : CustomViewModelBase
    {
        public IStationService StationService { get; set; }


        private Geolocator _instance;
        public Geolocator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Geolocator(); // 
                    _instance.DesiredAccuracy = PositionAccuracy.Default;
                    _instance.MovementThreshold = 250; // use MovementThreshold to ignore noise in the signal

                    _instance.StatusChanged += _instance_StatusChanged;
                    _instance.PositionChanged += _instance_PositionChanged;

                }

                return _instance;
            }
        }


        public GpsWatcherModel()
        {
            StationService = SimpleIoc.Default.GetInstance<IStationService>();

            
        }

      

        private Geocoordinate _currentLocation;

        public Geocoordinate CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value;
            RaisePropertyChanged(() => CurrentLocation);
            RaisePropertyChanged(() => CenterLocation);
            }
        }


        private IEnumerable<Station> _stations;
        public IEnumerable<Station> Stations
        {
            get {
                if (_stations != null)
                {
                    return _stations.Take(4);
                }
                else
                    return null;
            }
            set
            {
                _stations = value;

                RaisePropertyChanged(() => Stations);

            }
        }

       
        public Bing.Maps.Location CenterLocation
        {
            get
            {
                if (CurrentLocation == null)
                    return null;

                return new Bing.Maps.Location(CurrentLocation.Latitude, CurrentLocation.Longitude);
            }
        }


        async void _instance_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            try
            {

                try
                {
                    CurrentLocation = args.Position.Coordinate;
                }
                catch
                {
                }

                var stationList = await StationService.GetStations("NL");

                var list = await Task.Run(() =>
                {
                    foreach (var item in stationList)
                    {
                        CalcDistance(item);
                    }

                    return stationList.OrderBy(x => x.Distance).Take(10);
                });

                DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Stations = list;
                    CurrentLocation = _currentLocation;
                });

                //DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                //{
                //    CurrentLocation = args.Position.Coordinate;

                //});
            }
            catch { }
        }

        void _instance_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            bool isBusy = false;

            switch (args.Status)
            {
                case PositionStatus.Disabled:

                    break;
                case PositionStatus.Initializing:
                    isBusy = true;

                    break;
                case PositionStatus.NoData:
                    break;
                case PositionStatus.NotAvailable:
                    break;
                case PositionStatus.NotInitialized:
                    break;
                case PositionStatus.Ready:
                    break;
                default:
                    break;
            }

            if (DispatcherHelper.UIDispatcher != null)
            {
                DispatcherHelper.UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (isBusy)
                        LoadingState = ViewModel.LoadingState.Loading;
                    else
                        LoadingState = ViewModel.LoadingState.Finished;


                });
            }

            //Windows.Management.Deployment.Dispatcher.BeginInvoke(() => IsBusy = isBusy);
        }


       

       

        private void CalcDistance(Station station)
        {
            //Geocoordinate stationGps = new Geocoordinate(station.Lat, station.Long);
            if (CurrentLocation != null)
            {
                var dis = this.distance(CurrentLocation.Latitude, CurrentLocation.Longitude, station.Lat, station.Long, 'K');

                station.SetDistance(dis);
            }
        }

        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }



        internal void Initialize()
        {
            if (Instance == null)
            {
            }

          

        }


        public override void Cleanup()
        {
            base.Cleanup();

            if (_instance != null)
            {
                _instance.StatusChanged -= _instance_StatusChanged;
                _instance.PositionChanged -= _instance_PositionChanged;

                _instance = null;
            }
            
        }
       
    }
}
