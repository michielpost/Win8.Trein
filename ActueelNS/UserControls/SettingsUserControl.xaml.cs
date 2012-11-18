using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ApplicationSettings;
using ActueelNS.Services.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using ActueelNS.ViewModel;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ActueelNS.UserControls
{
    public sealed partial class SettingsUserControl : UserControl
    {
        private ISettingService _settingService;

        public SettingsUserControl()
        {
            this.InitializeComponent();

            _settingService = SimpleIoc.Default.GetInstance<ISettingService>();

            ShowHslToggle.IsOn = _settingService.GetShowHsl();
            UseOvToggle.IsOn = _settingService.GetOvCard();
        }

        private void OnBackButtonClicked(object sender, RoutedEventArgs e)
        {
            if (this.Parent.GetType() == typeof(Popup))
            {
                ((Popup)this.Parent).IsOpen = false;
            }
            SettingsPane.Show();
        }

        private void ShowHslToggle_Toggled_1(object sender, RoutedEventArgs e)
        {
            _settingService.SaveShowHsl(ShowHslToggle.IsOn);
        }

        private void UseOvToggle_Toggled_1(object sender, RoutedEventArgs e)
        {
            _settingService.SaveOvCard(UseOvToggle.IsOn);

        }

        private void DeleteSearchHistoryButton_Click_1(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<ISearchHistoryService>().Clear();

            SimpleIoc.Default.GetInstance<MainViewModel>().SearchHistory.Clear();
        }

       

    }
}
