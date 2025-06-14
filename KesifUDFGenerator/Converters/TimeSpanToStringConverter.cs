using System.Globalization;
using System.Windows.Data;

namespace KesifUDFGenerator.Converters;

/// <summary>
/// TimeSpan değerleri string'e çeviren converter
/// </summary>
public class TimeSpanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm");
        }
        return "15:30";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && TimeSpan.TryParseExact(stringValue, @"hh\:mm", CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }
        return new TimeSpan(15, 30, 0);
    }
}
