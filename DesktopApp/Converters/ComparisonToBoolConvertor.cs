using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DesktopApp.Converters;

public class ComparisonToBoolConvertor : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.Equals(parameter);
    }
}