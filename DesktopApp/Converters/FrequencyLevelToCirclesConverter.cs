using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using LongmanDictionary.Models;

namespace DesktopApp.Converters;

public class FrequencyLevelToCirclesConverter : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is FrequencyLevel freq)
        {
            return freq switch
            {
                FrequencyLevel.None => string.Empty,
                FrequencyLevel.Low => "●○○",
                FrequencyLevel.Medium => "●●○",
                FrequencyLevel.High => "●●●",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}