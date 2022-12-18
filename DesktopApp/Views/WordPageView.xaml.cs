using System.Windows;
using System.Windows.Controls;
using DesktopApp.ViewModels;
using LongmanDictionary.Models;

namespace DesktopApp.Views;

public partial class WordPageView
{
    public WordPageView()
    {
        InitializeComponent();
    }

    private WordPageViewModel ViewModel => (WordPageViewModel)DataContext;

    private void OnExampleChecked(object sender, RoutedEventArgs e)
    {
        var radioButton = (RadioButton)sender;
        if (radioButton.IsChecked ?? false)
            ViewModel.SelectedExample.Value = (Example)radioButton.DataContext;
        else
            ViewModel.SelectedExample.Value = null;
    }
}