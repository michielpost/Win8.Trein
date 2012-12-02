using System;
using System.Linq;
using System.Collections.Generic;

using Bing.Maps;
using Windows.Foundation;

namespace BingMaps.WinRT.Extensions
{
    public class PushpinClusterer
    {
        private readonly Map _map;
        private readonly MapItemsControl _mapItemsControl;

        private readonly List<IGeoCluster> _pins;
        private readonly int _distanceTreshold;
        
        private const int DefaultDistanceTreshold = 50;

        /// <summary>
        /// Cluster pins on the map
        /// </summary>
        public PushpinClusterer(Map map, MapItemsControl mapItemsControl, IEnumerable<IGeoCluster> pins, int distanceTreshold)
        {
            _map = map;
            _mapItemsControl = mapItemsControl;
            
            _map.Children.Add(_mapItemsControl);
            _map.ViewChangeEnded += (s, e) => RenderPins();
            
            _pins = pins.ToList();
            _distanceTreshold = distanceTreshold == 0 ? DefaultDistanceTreshold : distanceTreshold;
        }
        
        private async void RenderPins()
        {
            var pinsToAdd = new List<PushpinContainer>();

            foreach (var pin in _pins)
            {
                Point screenLoc;
                _map.TryLocationToPixel(new Location(pin.Latitude, pin.Longitude), out screenLoc);

                var newPinContainer = new PushpinContainer(pin, screenLoc);
                bool addNewPin = true;
               
                foreach (var pinContainer in pinsToAdd)
                {
                    double distance = ComputeDistance(pinContainer.ScreenLocation, newPinContainer.ScreenLocation);
                    
                    if (distance < _distanceTreshold)
                    {
                        pinContainer.Merge(newPinContainer);
                        addNewPin = false;
                        break;
                    }
                }

                if (addNewPin)
                {
                    pinsToAdd.Add(newPinContainer);
                }
            }
           
            await _map.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var items = new List<IGeoCluster>();
                foreach (var projectedPin in pinsToAdd.Where(pin => PointIsVisibleInMap(pin.ScreenLocation, _map)))
                {
                    items.Add(projectedPin.GetCluster());
                }

                _mapItemsControl.ItemsSource = items;
            });
        }
        
        private static bool PointIsVisibleInMap(Point point, Map map)
        {
            return point.X > 0 && point.X < map.ActualWidth &&
                    point.Y > 0 && point.Y < map.ActualHeight;
        }
        
        private double ComputeDistance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }
}
