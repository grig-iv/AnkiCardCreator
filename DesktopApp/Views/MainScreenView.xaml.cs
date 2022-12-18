using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DesktopApp.ViewModels;

namespace DesktopApp.Views;

public partial class MainScreenView : UserControl
{
    public MainScreenView()
    {
        InitializeComponent();
        SearchTextBox.Focus();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            ((MainScreenViewModel)DataContext).Word.Value = Clipboard.GetText();
    }
}