using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Input;
using DesktopApp.ViewModels;
using ReactiveUI;

namespace DesktopApp.Views;

public partial class MainScreenView
{
    public MainScreenView()
    {
        InitializeComponent();
        SearchTextBox.Focus();

        this.WhenActivated(disposableRegistration =>
        {
            this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.IsSearching,
                    view => view.SearchIndicator.Visibility)
                .DisposeWith(disposableRegistration);
        });
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            ((MainScreenViewModel) DataContext).Word = Clipboard.GetText();
    }
}