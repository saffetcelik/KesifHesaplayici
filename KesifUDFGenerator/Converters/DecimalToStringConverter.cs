using System.Globalization;
using System.Windows.Data;

namespace KesifUDFGenerator.Converters;

/// <summary>
/// Decimal değerleri string'e çeviren converter
/// </summary>
public class DecimalToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal decimalValue)
        {
            return decimalValue.ToString("F2", CultureInfo.CurrentCulture);
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue && decimal.TryParse(stringValue, NumberStyles.Number, CultureInfo.CurrentCulture, out var result))
        {
            return result;
        }
        return 0m;
    }
}
