﻿using System;
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
using ActueelNS.Views.Print;
using ActueelNS.UserControls;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ActueelNS.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TravelAdviceSinglePage : BasePrintPage
    {

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

        protected override void dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            //base.dtm_DataRequested(sender, args);
            
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

            ViewModel.Initialize((ReisMogelijkheid)e.Parameter);

           
           
            SuspensionManager.SessionState["PageType"] = typeof(TravelAdviceSinglePage).FullName;
            SuspensionManager.SessionState["PageArgs"] = e.Parameter;

            base.OnNavigatedTo(e);


        }

       
        private void Herplan_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SetPlannerSearch();

            this.Frame.Navigate(typeof(MainPage));
        }


        /// <summary>
        /// Provide print content for scenario 1 first page
        /// </summary>
        protected override void PreparetPrintContent()
        {
            if (firstPage == null)
            {
                firstPage = new PrintContentTest();
                firstPage.DataContext = ViewModel.ReisMogelijkheid.GetAsHtmlForPrint();
            }

            // Add the (newley created) page to the printing root which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            PrintingRoot.Children.Add(firstPage);
            PrintingRoot.InvalidateMeasure();
            PrintingRoot.UpdateLayout();
        }
      
    }
}
