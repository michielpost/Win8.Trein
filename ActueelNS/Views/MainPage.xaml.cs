using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ActueelNS.Common;
using ActueelNS.Services.Models;
using ActueelNS.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.UserProfile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ActueelNS.Views
{
    public enum SliderType
    {
        None,
        Van,
        Naar,
        Via
    }

    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : ActueelNS.Common.CustomBasePage
    {
        public MainViewModel ViewModel
        {
            get
            {
                return (MainViewModel)this.DataContext;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
            

            this.Loaded += MainPage_Loaded;

            //LocationList.Source = new LocationDataCollection() ;
            
        }


        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Initialize();

            //this.AddTextBox.Focus(Windows.UI.Xaml.FocusState.Programmatic);

            if (string.IsNullOrEmpty(Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride))
            {
                DisruptionList.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            this.addAutoComplete.InitializeSuggestionsProvider(this.ViewModel.Planner.SearchStationProvider);

        }

       

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SuspensionManager.SessionState["PageType"] = null;
            SuspensionManager.SessionState["PageArgs"] = null;
           
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ViewModel.SoftCleanup();

        }


        private void StoringenBorder_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StoringenPage));
        }

        private void TempToggle_Click_1(object sender, RoutedEventArgs e)
        {
            //semanticZoom.ToggleActiveView();
        }

        private void Stad_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            DepartureTimesViewModel station = (sender as FrameworkElement).DataContext as DepartureTimesViewModel;

            if (station != null)
            {
                this.Frame.Navigate(typeof(DepartureTimesPage), station.CurrentStation.Code);
            }
        }

       

        private void AddStation_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            AppBar.IsOpen = false;

            addAutoComplete.Text = string.Empty;

            AddStationPopUp.Visibility = Windows.UI.Xaml.Visibility.Visible;

            addAutoComplete.Focus(Windows.UI.Xaml.FocusState.Keyboard);


        }


        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            AddStationPopUp.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void ListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Station station = (Station)e.ClickedItem;

            if (station != null)
            {
                this.Frame.Navigate(typeof(DepartureTimesPage), station.Code);
            }
        }

        private void SearchHistoryList_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            SearchHistory sh = (SearchHistory)e.ClickedItem;

            if (sh != null)
            {
                this.Frame.Navigate(typeof(TravelAdvicePage), sh);
            }
        }

        private void TrajectList_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Traject t = (Traject)e.ClickedItem;

            if (t != null)
            {
                this.ViewModel.PickTraject(t);

                Messenger.Default.Send("", "highlight");
            }
        }

        private void TrajectList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            FavStationsList.SelectedItems.Clear();
            SearchHistoryList.SelectedItems.Clear();

            if (this.TrajectList.SelectedItems.Count > 0)
            {
                this.BottomAppBar.IsSticky = true;
                this.BottomAppBar.IsOpen = true;

                DeleteTraject.IsEnabled = true;
                DeleteTraject.Visibility  =  Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;

                DeleteTraject.IsEnabled = false;
                DeleteTraject.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }

        }

      

        private void FavStationsList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            TrajectList.SelectedItems.Clear();
            SearchHistoryList.SelectedItems.Clear();

            if (this.FavStationsList.SelectedItems.Count > 0)
            {
                this.BottomAppBar.IsSticky = true;
                this.BottomAppBar.IsOpen = true;

                DeleteStation.IsEnabled = true;
                DeleteStation.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;

                DeleteStation.IsEnabled = false;
                DeleteStation.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }
        }

        private void SearchHistoryList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            TrajectList.SelectedItems.Clear();
            FavStationsList.SelectedItems.Clear();

            if (this.SearchHistoryList.SelectedItems.Count > 0)
            {
                this.BottomAppBar.IsSticky = true;
                this.BottomAppBar.IsOpen = true;

                DeleteTrip.IsEnabled = true;
                DeleteTrip.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                this.BottomAppBar.IsOpen = false;
                this.BottomAppBar.IsSticky = false;

                DeleteTrip.IsEnabled = false;
                DeleteTrip.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            }
        }


        private void DeleteTraject_Click_1(object sender, RoutedEventArgs e)
        {
            var list = new List<Traject>();
            foreach (var t in this.TrajectList.SelectedItems)
            {
                list.Add((Traject)t);
            }

            ViewModel.DeleteTrajecten(list);

        }

        private void DeleteStation_Click_1(object sender, RoutedEventArgs e)
        {
            var list = new List<Station>();
            foreach (var t in this.FavStationsList.SelectedItems)
            {
                list.Add((Station)t);
            }

            ViewModel.DeleteStations(list);
        }

        private void DeleteTrip_Click_1(object sender, RoutedEventArgs e)
        {
            var list = new List<SearchHistory>();
            foreach (var t in this.SearchHistoryList.SelectedItems)
            {
                list.Add((SearchHistory)t);
            }

            ViewModel.DeleteSearchHistory(list);
        }

        private void addAutoComplete_SuggestionPopupClosed_1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addAutoComplete.Text))
            {
                Station station = SimpleIoc.Default.GetInstance<PlannerViewModel>().GetStationByName(addAutoComplete.Text);

                if (station != null)
                {
                    ViewModel.AddFavStation(station);

                    AddStationPopUp.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
        }

        private void Map_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapPage));
        }

        
        private void AddStationButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(addAutoComplete.Text))
            {
                Station station = SimpleIoc.Default.GetInstance<PlannerViewModel>().GetStationByName(addAutoComplete.Text);

                if (station != null)
                {
                    ViewModel.AddFavStation(station);

                    AddStationPopUp.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                }
            }
        }

       
    }
}
