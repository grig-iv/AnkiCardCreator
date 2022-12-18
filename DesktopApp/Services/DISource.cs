using System;
using System.Windows.Markup;

namespace DesktopApp.Services;

public class DISource : MarkupExtension
{
    public static IServiceProvider Services { get; set; }
    
    public Type Type { get; set; }

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        Services = serviceProvider;
    }
    
    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        return Services.GetService(Type);
    }
}