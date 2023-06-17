using System;
using System.Globalization;

namespace View.Converters
{
	public class Base64ToImageSourceConverter : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;
            byte[] decodedImg = System.Convert.FromBase64String(input);
            Stream memoryStream = new MemoryStream(decodedImg);
            ImageSource output = ImageSource.FromStream(() => memoryStream);
            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

