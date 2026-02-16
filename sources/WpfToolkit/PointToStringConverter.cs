using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DustInTheWind.WpfToolkit;

public class PointToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Point point)
        {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);
            return $"{x}, {y}";
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
