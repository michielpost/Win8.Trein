using Windows.Foundation;
using System.Collections.Generic;

namespace BingMaps.WinRT.Extensions
{
    public class PushpinContainer
    {
        private readonly List<IGeoCluster> _pushpins;
        public Point ScreenLocation { get; private set; }

        /// <summary>
        /// Creates a container for the given pushpin
        /// </summary>
        public PushpinContainer(IGeoCluster pushpin, Point location)
        {
            _pushpins = new List<IGeoCluster>();
            _pushpins.Add(pushpin);

            ScreenLocation = location;
        }

        /// <summary>
        /// Adds the pins from the given container
        /// </summary>
        public void Merge(PushpinContainer container)
        {
            foreach (var pin in container._pushpins)
            {
                _pushpins.Add(pin);
            }
        }

        /// <summary>
        /// Gets pin cluster
        /// </summary>
        public IGeoCluster GetCluster()
        {
            var cluster = _pushpins[0];
            cluster.ChildrenCount = _pushpins.Count == 1 ? 0 : _pushpins.Count;

            return cluster;
        }
    }
}
