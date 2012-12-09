using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ActueelNS.Common;
using ActueelNS.Services.Models;
using ActueelNS.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SearchResultsPage : ActueelNS.Common.CustomBasePage
    {

        public SearchResultsViewModel ViewModel
        {
            get
            {
                return (SearchResultsViewModel)this.DataContext;
            }
        }

        public SearchResultsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.SearchStation((string)e.Parameter);

            SuspensionManager.SessionState["PageType"] = typeof(SearchResultsPage).FullName;
            SuspensionManager.SessionState["PageArgs"] = e.Parameter;
        }

        internal void Search(string queryText)
        {
            ViewModel.SearchStation(queryText);
        }

        private void ListView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Station)
            {
                var station = e.ClickedItem as Station;

                App.RootFrame.Navigate(typeof(DepartureTimesPage), station.Code);
            }
        }
    }
}
