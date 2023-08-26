using System.Reactive.Disposables;
using System.Reactive.Linq;
using DesktopApp.ViewModels;
using ReactiveUI;

namespace DesktopApp.Views;

public partial class HeaderView
{
    public HeaderView()
    {
        InitializeComponent();
        ViewModel = (HeaderViewModel) DataContext;

        this.WhenActivated(disposables =>
        {
            SearchTextBox.Focus();

            this.Bind(
                    ViewModel,
                    viewModel => viewModel.Word,
                    view => view.SearchTextBox.Text)
                .DisposeWith(disposables);

            this.WhenAnyValue(x => x.SearchTextBox.Text)
                .Select(string.IsNullOrWhiteSpace)
                .BindTo(this, x => x.PlaceHolder.Visibility);

            this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.IsSearching,
                    view => view.SearchIndicator.Visibility)
                .DisposeWith(disposables);

            this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.SearchCommand,
                    view => view.SearchButton)
                .DisposeWith(disposables);

            this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.OpenLdoceCommand,
                    view => view.LogmanButton)
                .DisposeWith(disposables);
        });
    }
}