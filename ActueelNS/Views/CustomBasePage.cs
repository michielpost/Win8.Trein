using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Navigation;

namespace ActueelNS.Common
{
    public class CustomBasePage : ActueelNS.Common.LayoutAwarePage
    {
        private DataTransferManager dtm;
        protected ResourceLoader _resourceLoader = new ResourceLoader("Resources");


        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            dtm = DataTransferManager.GetForCurrentView();

            if (dtm != null)
            {
                dtm.DataRequested += dtm_DataRequested;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (dtm != null)
            {
                dtm.DataRequested -= dtm_DataRequested;
            }
        }



        protected virtual void dtm_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            args.Request.Data.Properties.Title = _resourceLoader.GetString("ShareDefaultTitle");

            //args.Request.Data.SetText(ViewModel.ReisMogelijkheid.GetAsText());
            //args.Request.Data.SetHtmlFormat(HtmlFormatHelper.CreateHtmlFormat(_resourceLoader.GetString("ShareDefaultText")));

            args.Request.FailWithDisplayText(_resourceLoader.GetString("ShareDefaultText"));
        }
    }
}
