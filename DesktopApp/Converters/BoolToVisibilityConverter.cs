using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DesktopApp.Converters;

public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
{
    public bool HideInsteadCollapse { get; set; }
    public bool Inverse { get; set; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return InverseIfNeeded(boolValue)
                ? Visibility.Visible
                : HideInsteadCollapse
                    ? Visibility.Hidden
                    : Visibility.Collapsed;

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private bool InverseIfNeeded(bool value)
    {
        return Inverse ? !value : value;
    }
}