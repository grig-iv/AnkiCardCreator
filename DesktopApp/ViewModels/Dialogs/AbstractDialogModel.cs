using System.Reactive;
using DesktopApp.ViewModels.Dialogs.Generic;

namespace DesktopApp.ViewModels.Dialogs
{
    public abstract class AbstractDialogModel : AbstractDialogModel<Unit>
    {
        protected AbstractDialogModel(string header) : base(header)
        {
        }

        protected override Unit OnAccept()
        {
            return Unit.Default;
        }
    }
}