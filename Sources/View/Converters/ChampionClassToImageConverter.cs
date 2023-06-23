using System;
using System.Globalization;
using ViewModel;

namespace View.Converters
{
    public class ChampionClassToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !ChampionVM.ClassesToStringImages.ContainsKey(value.ToString())) {
                return null;
            }
            return ChampionVM.ClassesToStringImages[value.ToString()];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

