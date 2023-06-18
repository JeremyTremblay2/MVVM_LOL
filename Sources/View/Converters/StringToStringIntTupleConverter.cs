using System;
using System.Globalization;

namespace View.Converters
{
    public class StringToStringIntTupleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is string key && int.TryParse(values[1] as string, out int value) && values.Length == 2)
            {
                return new Tuple<string, int>(key, value);
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

