namespace BingMaps.WinRT.Extensions
{
    /// <summary>
    /// Interface that every location based object need to inherit if
    /// it want to be clustered
    /// </summary>
    public interface IGeoCluster
    {
        /// <summary>
        /// Latitude of the cluster point
        /// </summary>
        double Latitude { get; set; }

        /// <summary>
        /// Longitude of the cluster point
        /// </summary>
        double Longitude { get; set; }

        /// <summary>
        /// Number of cluster children (items in cluster container)
        /// </summary>
        int ChildrenCount { get; set; }
    }
}
