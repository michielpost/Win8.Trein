using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ActueelNS.UserControls
{
    public sealed partial class ReisadviesSingleControl : UserControl
    {
        public ReisadviesSingleControl()
        {
            this.InitializeComponent();
        }

        private async void invokePrintingButton_Click_1(object sender, RoutedEventArgs e)
        {
            // Don't act when in snapped mode
            if (ApplicationView.Value != ApplicationViewState.Snapped)
            {
                try
                {
                    await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();
                }
                catch { }
            }
        }
    }
}
