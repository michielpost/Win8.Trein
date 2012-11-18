using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ActueelNS.Services.Models;
using ActueelNS.ViewModel;
using ActueelNS.Views;
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
using Syncfusion.UI.Xaml.Controls.Input;
using System.Collections;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ActueelNS.UserControls
{
    public sealed partial class PlannerControl : UserControl
    {

        public PlannerViewModel ViewModel
        {
            get
            {
                return (PlannerViewModel)this.DataContext;
            }
        }

        public PlannerControl()
        {
            this.InitializeComponent();

            this.Loaded += PlannerControl_Loaded;

            Messenger.Default.Register<string>(this, "highlight", DoHighlight);

            Messenger.Default.Register<string>(this, "setVan", DoSetVan);
            Messenger.Default.Register<string>(this, "setNaar", DoSetNaar);
            Messenger.Default.Register<string>(this, "setVia", DoSetVia);



        }

        void DoHighlight(string input)
        {
            Highlight.Begin();
        }

        async void DoSetVan(string input)
        {
            fromAutoComplete.Text = input;

            await Task.Delay(10);
            fromAutoComplete.IsSuggestionOpen = false;

        }

        async void DoSetNaar(string input)
        {
            toAutoComplete.Text = input;

            await Task.Delay(10);
            toAutoComplete.IsSuggestionOpen = false;

        }

        async void DoSetVia(string input)
        {
            viaAutoComplete.Text = input;

            await Task.Delay(10);
            viaAutoComplete.IsSuggestionOpen = false;

        }

        void PlannerControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            //this.VanTextBox.Focus(Windows.UI.Xaml.FocusState.Programmatic);

            DatePicker.MinValue = DateTime.Now.AddDays(-7);
            DatePicker.MaxValue = DateTime.Now.AddYears(1);
        }


        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsViaCustomVisible"
                && ViewModel.IsViaVisible)
            {
                this.viaAutoComplete.Focus(Windows.UI.Xaml.FocusState.Programmatic);
            }


            if (e.PropertyName == "ShowFromError")
            {
                if (ViewModel.ShowFromError)
                {
                    this.FromErrorBorder.BorderThickness = new Thickness(1);
                    StationErrorTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                    this.FromErrorBorder.BorderThickness = new Thickness(0);

                if (!ViewModel.ShowFromError
                    && !ViewModel.ShowToError)
                {
                    StationErrorTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                }

            }

            if (e.PropertyName == "ShowToError")
            {
                if (ViewModel.ShowToError)
                {
                    this.ToErrorBorder.BorderThickness = new Thickness(1);
                    StationErrorTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                    this.ToErrorBorder.BorderThickness = new Thickness(0);

                if (!ViewModel.ShowFromError
                    && !ViewModel.ShowToError)
                {
                    StationErrorTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                }

            }


        }


        private void NowTextBox_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            NowTextBoxTap();
        }

        private void NowTextBoxTap()
        {
            if (ViewModel.PlanDateTime.HasValue)
            {
                ViewModel.PlanDateTime = null;

                HideDatePicker.Begin();
            }
            else
            {
                ViewModel.SetDateNow();

                ShowDatePicker.Begin();
            }
        }

    

        private void WisselImage_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.FinishAutoComplete(InputType.None, string.Empty);

            this.ViewModel.WisselVanNaar();
        }


        private void from_SuggestionPopupClosed_1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.FinishAutoComplete(InputType.Van, fromAutoComplete.Text);
        }

        private void fromAutoComplete_LostFocus_1(object sender, RoutedEventArgs e)
        {
            ViewModel.FinishAutoComplete(InputType.Van, fromAutoComplete.Text);
        }


        private void to_SuggestionPopupClosed_1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.FinishAutoComplete(InputType.Naar, toAutoComplete.Text);
        }

        private void toAutoComplete_LostFocus_1(object sender, RoutedEventArgs e)
        {
            ViewModel.FinishAutoComplete(InputType.Naar, toAutoComplete.Text);
        }


        private void via_SuggestionPopupClosed_1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ViewModel.FinishAutoComplete(InputType.Via, viaAutoComplete.Text);
        }

        private void viaAutoComplete_LostFocus_1(object sender, RoutedEventArgs e)
        {
            ViewModel.FinishAutoComplete(InputType.Via, viaAutoComplete.Text);
        }
      
    }

    

}
