using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ActueelNS.Common;
using ActueelNS.Services.Models;
using ActueelNS.ViewModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
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
    public sealed partial class TravelAdviceSinglePage : ActueelNS.Common.LayoutAwarePage
    {

        private ResourceLoader _resourceLoader = new ResourceLoader("Resources");

        private DataTransferManager dtm;

        public TravelAdviceSingleViewModel ViewModel
        {
            get
            {
                return (TravelAdviceSingleViewModel)this.DataContext;
            }
        }

        public TravelAdviceSinglePage()
        {
            this.InitializeComponent();

           // this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;
            
        }

    


        void dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {

            if (ViewModel != null)
            {
            args.Request.Data.Properties.Title = string.Format("{0} ({1})", ViewModel.ReisMogelijkheid.VanNaar, ViewModel.ReisMogelijkheid.VanTot);

            //args.Request.Data.SetText(ViewModel.ReisMogelijkheid.GetAsText());
            args.Request.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(ViewModel.ReisMogelijkheid.GetAsHtml()));

            }
            else
            {
                args.Request.FailWithDisplayText(_resourceLoader.GetString("ShareError"));
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.Initialize((ReisMogelijkheid)e.Parameter);

            dtm = DataTransferManager.GetForCurrentView();

            if (dtm != null)
            {
                dtm.DataRequested += dtm_DataRequested;
            }
           
            SuspensionManager.SessionState["PageType"] = typeof(TravelAdviceSinglePage).FullName;
            SuspensionManager.SessionState["PageArgs"] = e.Parameter;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (dtm != null)
            {
                dtm.DataRequested -= dtm_DataRequested;
            }
        }

        private void Herplan_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SetPlannerSearch();

            this.Frame.Navigate(typeof(MainPage));
        }


      
    }
}
