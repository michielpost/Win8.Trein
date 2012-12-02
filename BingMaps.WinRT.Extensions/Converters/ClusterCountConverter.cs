using System;
using Windows.UI.Xaml.Data;

namespace BingMaps.WinRT.Extensions.Converters
{
    public class ClusterCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var count = (int)value;
            return count > 0 ? count.ToString() : String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
