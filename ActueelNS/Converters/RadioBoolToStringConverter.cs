using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ActueelNS.Converters
{
    public class RadioBoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            string integer = (string)value;
            if (integer == parameter.ToString())
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            if ((bool)value)
                return parameter;
            else
                return null;
        }
    }
}
