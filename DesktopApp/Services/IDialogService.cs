using System;
using DesktopApp.ViewModels.Dialogs;
using DesktopApp.ViewModels.Dialogs.Generic;

namespace DesktopApp.Services
{
    public interface IDialogService
    {
        bool IsOpen { get; }

        IObservable<T>  Show<T>(AbstractDialogModel<T> dialogModel);
        IObservable<T>  Show<T>(AbstractDialogModel<T> dialogModel, string dialogViewName);
        void CloseCurrentDialog();
    }
}