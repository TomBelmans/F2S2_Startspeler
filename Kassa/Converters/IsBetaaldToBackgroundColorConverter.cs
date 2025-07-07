namespace Kassa.Converters
{
    public class IsBetaaldToBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool bestellingVerwerkt)
            {
                return bestellingVerwerkt ? Color.FromRgba(0, 255, 0, 0.5) : Color.FromRgba(255, 0, 0, 0.5);
            }
            return Color.FromRgba(128, 128, 128, 0.5);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
