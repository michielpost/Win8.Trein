using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ActueelNS.Common;
using ActueelNS.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ActueelNS.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class DepartureTimesPage : ActueelNS.Common.LayoutAwarePage
    {

        public DepartureTimesViewModel ViewModel
        {
            get
            {
                return (DepartureTimesViewModel)this.DataContext;
            }
        }

        public DepartureTimesPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;


        }

       

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!string.IsNullOrEmpty((string)e.Parameter))
                ViewModel.Initialize((string)e.Parameter, true);
            
            SuspensionManager.SessionState["PageType"] = typeof(DepartureTimesPage).FullName;
            SuspensionManager.SessionState["PageArgs"] = e.Parameter;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            ViewModel.Cleanup();
        }

        private async void Pin_Tapped_1(object sender, TappedRoutedEventArgs e)
        {

            SecondaryTile secondaryTile = ViewModel.CreateTileForStation();

              // OK, the tile is created and we can now attempt to pin the tile. 
            // Note that the status message is updated when the async operation to pin the tile completes. 
            bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Windows.UI.Popups.Placement.Above);

            ViewModel.SetPinned(isPinned);
        }

        Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private async void UnPin_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            SecondaryTile secondaryTile = ViewModel.CreateTileForStation();

            // Now make the delete request.
            bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender), Windows.UI.Popups.Placement.Below);

            ViewModel.SetPinned(!isUnpinned);

        }

        private void Plan_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SetStationForPlanner();
           

            this.Frame.Navigate(typeof(MainPage));
        }

       
    }
}
