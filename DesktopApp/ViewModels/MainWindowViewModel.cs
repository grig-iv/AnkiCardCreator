using MaterialDesignThemes.Wpf;

namespace DesktopApp.ViewModels;

public class MainWindowViewModel
{
    public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
    {
        SnackbarMessageQueue = snackbarMessageQueue;
    }

    public ISnackbarMessageQueue SnackbarMessageQueue { get; }
}