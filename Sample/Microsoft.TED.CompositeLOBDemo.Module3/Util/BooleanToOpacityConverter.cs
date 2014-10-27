using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.TED.CompositeLOBDemo.Module3.Util
{
    public sealed class BooleanToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToBoolean(value);
            if (val)
                return 0.2;
            else
                return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var val = System.Convert.ToDouble(value);
            if (val <= 0.2)
                return false;
            else
                return true;
        }
    }
}
