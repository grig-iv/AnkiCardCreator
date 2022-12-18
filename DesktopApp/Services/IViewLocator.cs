using System.Windows.Controls;

namespace DesktopApp.Services;

public interface IViewLocator
{
    Control ResolveView(string viewName);
}