using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

using Bing.Maps;
using Windows.UI.Xaml;

namespace BingMaps.WinRT.Extensions
{
    /// <summary>
    /// Map extensions (atached properties)
    /// </summary>
    public class MapExtensions : DependencyObject
    {
        #region ClusteredItemsSource property
        public static readonly DependencyProperty ClusteredItemsSourceProperty =
        DependencyProperty.RegisterAttached(
            "ClusteredItemsSource",
            typeof(IEnumerable),
            typeof(MapExtensions),
            new PropertyMetadata(null, OnItemsReceived));

        public static void SetClusteredItemsSource(DependencyObject target, IEnumerable<IGeoCluster> value)
        {
            target.SetValue(ClusteredItemsSourceProperty, value);
        }

        public static IEnumerable<IGeoCluster> GetClusteredItemsSource(DependencyObject target)
        {
            return target.GetValue(ClusteredItemsSourceProperty) as IEnumerable<IGeoCluster>;
        }

        private static void OnItemsReceived(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var map = d as Map;
            if (map == null) return;

            var mapItems = new MapItemsControl();
            mapItems.ItemTemplate = d.GetValue(ClusterItemTemplateProperty) as DataTemplate;

            var distanceProperty = (int)d.GetValue(ClusterDistanceProperty);

            var items = e.NewValue as IEnumerable<IGeoCluster>;
            var cluster = new PushpinClusterer(map, mapItems, items, distanceProperty);
        }
        #endregion

        #region ClusterItemTemplate property
        public static readonly DependencyProperty ClusterItemTemplateProperty =
       DependencyProperty.RegisterAttached(
           "ClusterItemTemplate",
           typeof(DataTemplate),
           typeof(MapExtensions),
           new PropertyMetadata(null));

        public static void SetClusterItemTemplate(DependencyObject target, DataTemplate value)
        {
            target.SetValue(ClusterItemTemplateProperty, value);
        }

        public static DataTemplate GetClusterItemTemplate(DependencyObject target)
        {
            return target.GetValue(ClusteredItemsSourceProperty) as DataTemplate;
        }
        #endregion

        #region ClusterDistance property
        public static readonly DependencyProperty ClusterDistanceProperty =
                 DependencyProperty.RegisterAttached(
                 "ClusterDistance",
                 typeof(int),
                 typeof(MapExtensions),
                 new PropertyMetadata(null));

        public static void SetClusterDistance(DependencyObject attached, int value)
        {
            attached.SetValue(ClusterDistanceProperty, value);
        }

        public static int GetClusterDistance(DependencyObject attached)
        {
            return (int)attached.GetValue(ClusterDistanceProperty);
        }
        #endregion

        #region Command property
        public static readonly DependencyProperty CommandProperty =
                 DependencyProperty.RegisterAttached(
                 "Command",
                 typeof(ICommand),
                 typeof(MapExtensions),
                 new PropertyMetadata(null, CommandPropertyChanged));

        public static void SetCommand(DependencyObject attached, ICommand value)
        {
            attached.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject attached)
        {
            return (ICommand)attached.GetValue(CommandProperty);
        }

        private static void CommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (d as FrameworkElement);
            if (element == null) return;

            element.Tapped += ItemTapCommandTapped;
        }

        private static void ItemTapCommandTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            // Get command
            var element = sender as DependencyObject;
            if (element == null) return;

            var command = GetCommand(element);
            var parameter = element.GetValue(CommandParameterProperty);

            // Execute command
            command.Execute(parameter);
        }
        #endregion

        #region CommandParameter property
        public static readonly DependencyProperty CommandParameterProperty =
                 DependencyProperty.RegisterAttached(
                 "CommandParameter",
                 typeof(Object),
                 typeof(MapExtensions),
                 new PropertyMetadata(null));

        public static void SetCommandParameter(DependencyObject attached, object value)
        {
            attached.SetValue(CommandParameterProperty, value);
        }

        public static object GetCommandParameter(DependencyObject attached)
        {
            return (object)attached.GetValue(CommandProperty);
        }
        #endregion
    }
}
