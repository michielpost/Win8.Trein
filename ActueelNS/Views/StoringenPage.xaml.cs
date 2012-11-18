using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ActueelNS.Common;
using ActueelNS.Helpers;
using ActueelNS.Services.Models;
using ActueelNS.UserControls;
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
    public sealed partial class StoringenPage : ActueelNS.Common.LayoutAwarePage
    {
        private SettingsFlyout _flyOut = new SettingsFlyout();

        public StoringenPage()
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

            SuspensionManager.SessionState["PageType"] = typeof(StoringenPage).FullName;
            SuspensionManager.SessionState["PageArgs"] = e.Parameter;

        }

        private void WerkMainListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //Werkzaamheden w = WerkMainListBox.SelectedItem as Werkzaamheden;

            //if (w != null)
            //{
            //    w.IsExpanded = !w.IsExpanded;
            //}

            //WerkMainListBox.SelectedItem = null;
        }

        private void Storingen_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Storing storing = e.ClickedItem as Storing;

            if (storing != null)
            {
                _flyOut.ShowFlyout(new StoringenFlyOutControl() { DataContext = storing });
            }
        }

        private void Werk_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            Werkzaamheden werk = e.ClickedItem as Werkzaamheden;

            if (werk != null)
            {
                _flyOut.ShowFlyout(new WerkFlyOutControl() { DataContext = werk });
            }
        }
    }
}
