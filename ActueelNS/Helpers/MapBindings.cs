using Bing.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ActueelNS.Helpers
{
    
        public static class MapBindings
        {
            public static Location GetMapLocation(DependencyObject obj)
            {
                return (Location)obj.GetValue(MapLocationProperty);
            }

            public static void SetMapLocation(DependencyObject obj, Location value)
            {
                obj.SetValue(MapLocationProperty, value);
            }

            public static readonly DependencyProperty MapLocationProperty = DependencyProperty.RegisterAttached("MapLocation", typeof(Location), typeof(MapBindings), new PropertyMetadata(null, OnMapLocationChanged));

            private static void OnMapLocationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                if (d != null)
                {
                    MapLayer.SetPosition(d, (Location)e.NewValue);
                }
            }

            public static Location GetMapCenter(DependencyObject obj)
            {
                return (Location)obj.GetValue(MapCenterProperty);
            }

            public static void SetMapCenter(DependencyObject obj, Location value)
            {
                obj.SetValue(MapCenterProperty, value);
            }

            public static readonly DependencyProperty MapCenterProperty = DependencyProperty.RegisterAttached("MapCenter", typeof(Location), typeof(MapBindings), new PropertyMetadata(null, OnMapCenterChanged));

            private static void OnMapCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var map = d as Bing.Maps.Map;
                if (map != null)
                {
                    map.Center = (Location)e.NewValue;
                }
            }
        }

}
