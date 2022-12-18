namespace DesktopApp.ViewModels.Dialogs
{
    public class MessageDialogModel : AbstractDialogModel
    {
        public MessageDialogModel(string header, string message) : base(header)
        {
            Message = message;
        }

        public string Message { get; }

        public static MessageDialogModel ErrorDialog(string errorMessage)
        {
            return new MessageDialogModel("ERROR", errorMessage)
            {
                IsOneButtonDialog = true,
                AcceptCaption = "OK",
            };
        }

        public static MessageDialogModel WarningDialog(string errorMessage)
        {
            return new MessageDialogModel("Warning", errorMessage)
            {
                IsOneButtonDialog = true,
                AcceptCaption = "OK",
            };
        }
    }
}