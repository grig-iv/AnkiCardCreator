using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using DesktopApp.Views;
using LongmanDictionary.Services;

namespace DesktopApp.Services;

public class ViewLocator : IViewLocator
{
    private readonly IReadOnlyDictionary<string, Type> _viewTypes;
    
    public ViewLocator()
    {
        _viewTypes = Assembly
            .GetAssembly(typeof(MainWindow))!
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(Control)) && t.GetConstructor(Type.EmptyTypes) != null)
            .ToDictionary(t => t.Name);
    }
    
    public Control ResolveView(string viewName)
    {
        var viewType = _viewTypes[viewName];
        return (Control)Activator.CreateInstance(viewType)!;
    }
}