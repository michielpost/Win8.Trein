using System;
using System.Linq;
using System.Threading.Tasks;
using ActueelNS.Common;
using ActueelNS.Helpers;
using ActueelNS.Services.Interfaces;
using ActueelNS.Services.Models;
using ActueelNS.UserControls;
using ActueelNS.ViewModel;
using ActueelNS.Views;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Search;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Telerik.UI.Xaml.Controls.Input;
using Windows.ApplicationModel.Resources.Core;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace ActueelNS
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private bool _isSearchReg;
        private bool _isSettingsReg;

        public static Frame RootFrame;

        // This is the SearchPane object
        private SearchPane searchPane;

        private ResourceLoader _resourceLoader = new ResourceLoader("Resources");

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += App_Resuming;

            InputLocalizationManager.Instance.UserResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");

        }

        void App_Resuming(object sender, object e)
        {
            SimpleIoc.Default.GetInstance<MainViewModel>().Initialize();
            SimpleIoc.Default.GetInstance<DepartureTimesViewModel>().SoftInitialize();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            //Set language
            DetectDutchLanguage();

            DispatcherHelper.Initialize();

            SuspensionManager.KnownTypes.Add(typeof(PlannerSearch));
            SuspensionManager.KnownTypes.Add(typeof(Station));
            SuspensionManager.KnownTypes.Add(typeof(ReisMogelijkheid));
            SuspensionManager.KnownTypes.Add(typeof(ReisDeel));
            SuspensionManager.KnownTypes.Add(typeof(ReisStop));

            // Create a Frame to act navigation context and navigate to the first page
            RootFrame = new Frame();

            Type pageType = null;
            object arg = null;

            if (!string.IsNullOrEmpty(args.Arguments))
            {
                if (args.Arguments.Contains("DepartureTimesPage"))
                {
                    string stationCode = args.Arguments.Replace("DepartureTimesPage=", string.Empty);

                    pageType = typeof(DepartureTimesPage);
                    arg = stationCode;
                }
            }
            else if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Load state from previously suspended application
                //     Do an asynchronous restore
                await SuspensionManager.RestoreAsync();

                string type = string.Empty;

                if (SuspensionManager.SessionState.ContainsKey("PageType") && SuspensionManager.SessionState["PageType"] != null)
                    type = (string)SuspensionManager.SessionState["PageType"];

                if (SuspensionManager.SessionState.ContainsKey("PageArgs") && SuspensionManager.SessionState["PageArgs"] != null)
                    arg = SuspensionManager.SessionState["PageArgs"];

                if (!string.IsNullOrEmpty(type))
                {
                    //pageType = typeof(App).GetTypeInfo().Assembly.GetType(type);
                    //pageType = System.Reflection.Assembly.Load(new AssemblyName(typeof(App).AssemblyQualifiedName)).GetType(type);
                    pageType = Type.GetType(type);

                }
            }

            RootFrame.Navigate(typeof(MainPage));

            if (pageType != null)
                RootFrame.Navigate(pageType, arg);


            // Place the frame in the current Window and ensure that it is active
            Window.Current.Content = RootFrame;
            Window.Current.Activate();

            RegisterForSettings();

            RegisterForSearch();
        }

        private void RegisterForSettings()
        {
            if (!_isSettingsReg)
            {
                _isSettingsReg = true;

                try
                {
                    SettingsPane.GetForCurrentView().CommandsRequested -= App_CommandsRequested;
                }
                catch { }
                SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;
            }
        }

        private static void DetectDutchLanguage()
        {
            bool forceDutch = false;
            var userlanguages = GlobalizationPreferences.Languages;
            if (userlanguages.Contains("nl-NL"))
            {
                forceDutch = true;
            }

            GeographicRegion userRegion = new GeographicRegion();
            if (userRegion.CodeTwoLetter.ToLower() == "nl")
            {
                forceDutch = true;
            }

            if (forceDutch)
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "nl";
            else
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = string.Empty;
        }

        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (args.Request.ApplicationCommands.Count == 0)
            {
                // Add an About command
                var about = new SettingsCommand("about", _resourceLoader.GetString("AppBarAboutTitle"), (handler) =>
                {
                    var settings = new SettingsFlyout();
                    settings.ShowFlyout(new AboutUserControl());
                });
                args.Request.ApplicationCommands.Add(about);


                // Add an settings command
                var settingsCommand = new SettingsCommand("settings", _resourceLoader.GetString("AppBarSettingsTitle"), (handler) =>
                {
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.ShowFlyout(new SettingsUserControl());
                });
                args.Request.ApplicationCommands.Add(settingsCommand);

                // Add an privacy policy command
                var privacyCommand = new SettingsCommand("privacy", "Privacy Policy", (handler) =>
                {
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.ShowFlyout(new PrivacyPolicyControl());
                });
                args.Request.ApplicationCommands.Add(privacyCommand);
            }

        }


        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            //TODO: Save application state and stop any background activity
            ViewModelLocator.Cleanup();

            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }


        async protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            base.OnSearchActivated(args);

            //Set language
            DetectDutchLanguage();

            EnsureMainPageAsync(args);
            //((MainPage)Window.Current.Content).Test(args.QueryText);

            RegisterForSettings();

            RegisterForSearch();

            await SearchStations(args.QueryText);
        }

        private void EnsureMainPageAsync(IActivatedEventArgs args)
        {

            // If the window isn't already using Frame navigation, insert our own frame
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;
            if (frame == null)
            {
                DispatcherHelper.Initialize();

                // Create a Frame to act navigation context and navigate to the first page
                RootFrame = new Frame();

                RootFrame.Navigate(typeof(MainPage));

                // Place the frame in the current Window and ensure that it is active
                Window.Current.Content = RootFrame;
                Window.Current.Activate();
            }
            // Use navigation to display the results, packing both the query text and the previous
            // Window content into a single parameter object
            //frame.Navigate(typeof(MainPage));
            // The window must be activated in 15 seconds
            //Window.Current.Activate();

        }

        private void RegisterForSearch()
        {
            if (!_isSearchReg)
            {
                _isSearchReg = true;

                // Get Search Pane object
                this.searchPane = SearchPane.GetForCurrentView();
                searchPane.PlaceholderText = _resourceLoader.GetString("AppSearchPlaceHolder");

                // Register for Search Pane QuerySubmitted event
                try
                {
                    this.searchPane.QuerySubmitted -= searchPane_QuerySubmitted;
                }
                catch { }
                this.searchPane.QuerySubmitted += searchPane_QuerySubmitted;
                //this.searchPane.QueryChanged += searchPane_QueryChanged;

                try
                {
                    this.searchPane.SuggestionsRequested -= searchPane_SuggestionsRequested;
                }
                catch { }
                this.searchPane.SuggestionsRequested += searchPane_SuggestionsRequested;
            }
        }

        async void searchPane_SuggestionsRequested(SearchPane sender, SearchPaneSuggestionsRequestedEventArgs args)
        {
            var stationService = SimpleIoc.Default.GetInstance<IStationService>();
            string queryText = args.QueryText;

            if (stationService != null && !string.IsNullOrEmpty(queryText))
            {
                var all = await stationService.GetStations("NL");
                var filtered = all.Where(x => x.Name.StartsWith(queryText, StringComparison.CurrentCultureIgnoreCase)).Take(5);

                foreach (var r in filtered)
                {
                    args.Request.SearchSuggestionCollection.AppendQuerySuggestion(r.Name);
                }

            }
        }


        async void searchPane_QuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            string queryText = args.QueryText;

            await SearchStations(queryText);

        }

        private static async Task SearchStations(string queryText)
        {
            var stationService = SimpleIoc.Default.GetInstance<IStationService>();

            if (stationService != null && !string.IsNullOrEmpty(queryText))
            {
                var all = await stationService.GetStations("NL");
                var filtered = all.Where(x => x.Name.StartsWith(queryText, StringComparison.CurrentCultureIgnoreCase)).Take(2);

                if (filtered.Count() < 2)
                {
                    var extraStations = all.Where(x => x.StartsWith(queryText)).Take(2);

                    filtered = filtered.Union(extraStations);
                }

                if (filtered.Count() == 1)
                {
                    RootFrame.Navigate(typeof(DepartureTimesPage), filtered.First().Code);
                }
                else
                {
                    if (RootFrame.Content is SearchResultsPage)
                    {
                        ((SearchResultsPage)RootFrame.Content).Search(queryText);
                    }
                    else
                        RootFrame.Navigate(typeof(SearchResultsPage), queryText);
                }

            }
        }
    }
}
