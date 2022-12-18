using DesktopApp.ViewModels.Dialogs;

namespace DesktopApp.Services
{
    public static class DialogServiceExtension
    {
        public static void ShowErrorDialog(this IDialogService dialogService, string message)
        {
            dialogService.Show(
                MessageDialogModel.ErrorDialog(message)
            );
        }
        
        public static void ShowWarningDialog(this IDialogService dialogService, string message)
        {
            dialogService.Show(
                MessageDialogModel.WarningDialog(message)
            );
        }
    }
}