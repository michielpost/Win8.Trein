﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ActueelNS.Common;
using ActueelNS.Services.Models;
using ActueelNS.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using ActueelNS.Views.Print;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace ActueelNS.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TravelAdvicePage : BasePrintPage
    {
        private ReisMogelijkheid _lastViewed;

        public TravelAdviceViewModel ViewModel
        {
            get
            {
                return (TravelAdviceViewModel)this.DataContext;
            }
        }

        public TravelAdvicePage()
        {
            this.InitializeComponent();

            Messenger.Default.Register<ReisMogelijkheid>(this, "SetOptimaal", SetOptimaal);

            this.Loaded += TravelAdvicePage_Loaded;
        }

        void TravelAdvicePage_Loaded(object sender, RoutedEventArgs e)
        {
            if(_lastViewed != null)
                SetOptimaal(_lastViewed);
            else if (this.ViewModel != null && this.ViewModel.ReisMogelijkheidOptimaal != null)
            {
                var optimaal = ViewModel.ReisMogelijkheidOptimaal;

                if (optimaal != null)
                    SetOptimaal(optimaal);
            }
        }


        //public override void ViewStateChanged(Windows.UI.ViewManagement.ApplicationView sender, Windows.UI.ViewManagement.ApplicationViewStateChangedEventArgs e)
        //{
        //        base.ViewStateChanged(sender, e);

        //        this.pageTitle.UpdateLayout();

        //}

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if (e.NavigationMode != NavigationMode.Back)
            {
                if(e.Parameter is PlannerSearch)
                    ViewModel.Initialize((PlannerSearch)e.Parameter);
                else if(e.Parameter is SearchHistory)
                    ViewModel.Initialize((SearchHistory)e.Parameter);

            }
            else
            {
                var model = SimpleIoc.Default.GetInstance<TravelAdviceSingleViewModel>();
                if (model.ReisMogelijkheid != null)
                    _lastViewed = model.ReisMogelijkheid;

                //    var optimaal = ViewModel.ReisMogelijkheden.Where(x => x.Optimaal).FirstOrDefault();

                //    if (optimaal != null)
                //        SetOptimaal(optimaal);
            }

            base.OnNavigatedTo(e);

            SuspensionManager.SessionState["PageType"] = typeof(TravelAdvicePage).FullName;

            if(e.Parameter is PlannerSearch)
                SuspensionManager.SessionState["PageArgs"] = (PlannerSearch)e.Parameter;
        }

        private void SetOptimaal(ReisMogelijkheid item)
        {
            //Large gridview
            MogelijkhedenGridView.UpdateLayout();

            MogelijkhedenGridView.ScrollIntoView(MogelijkhedenGridView.Items.LastOrDefault());

            MogelijkhedenGridView.UpdateLayout();
            MogelijkhedenGridView.ScrollIntoView(item);
            MogelijkhedenGridView.UpdateLayout();


            //Small list
            MogelijkhedenSmallList.UpdateLayout();

            MogelijkhedenSmallList.ScrollIntoView(MogelijkhedenSmallList.Items.LastOrDefault());

            MogelijkhedenSmallList.UpdateLayout();
            MogelijkhedenSmallList.ScrollIntoView(item);
            MogelijkhedenSmallList.UpdateLayout();

            


        }

        private void GridView_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            ReisMogelijkheid advies = e.ClickedItem as ReisMogelijkheid;

            if (advies != null)
            {
                this.Frame.Navigate(typeof(TravelAdviceSinglePage), advies);
            }
        }

        private void Herplan_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SetPlannerSearch();

            this.Frame.Navigate(typeof(MainPage));
        }

        protected override void dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            //base.dtm_DataRequested(sender, args);

            if (ViewModel != null)
            {
                args.Request.Data.Properties.Title = string.Format("{0}", ViewModel.ReisMogelijkheidOptimaal.VanNaar);


                string html = string.Empty;
                foreach (var reis in ViewModel.ReisMogelijkheden)
                {
                    html += reis.GetAsHtml();
                }

                //args.Request.Data.SetText(ViewModel.ReisMogelijkheid.GetAsText());
                args.Request.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(html));

            }
            else
            {
                args.Request.FailWithDisplayText(_resourceLoader.GetString("ShareError"));
            }
        }

        /// <summary>
        /// Provide print content for scenario 1 first page
        /// </summary>
        protected override void PreparetPrintContent()
        {
            if (ViewModel == null || ViewModel.ReisMogelijkheden == null)
                return;

            if (firstPage == null)
            {
                firstPage = new PrintContentTest();

                string html = string.Empty;
                foreach (var reis in ViewModel.ReisMogelijkheden)
                {
                    html += reis.GetAsHtmlForPrint();
                }

                firstPage.DataContext = html;
            }

            // Add the (newley created) page to the printing root which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            PrintingRoot.Children.Add(firstPage);
            PrintingRoot.InvalidateMeasure();
            PrintingRoot.UpdateLayout();
        }
     
    }
}
