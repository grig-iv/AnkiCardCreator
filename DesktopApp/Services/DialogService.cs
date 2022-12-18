using System;
using System.Windows;
using System.Windows.Controls;
using DesktopApp.ViewModels.Dialogs.Generic;

namespace DesktopApp.Services
{
    public class DialogService : DependencyObject, IDialogService
    {
        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            nameof(IsOpen), typeof(bool), typeof(DialogService), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty DialogViewProperty = DependencyProperty.Register(
            nameof(DialogView), typeof(Control), typeof(DialogService), new PropertyMetadata(default(Control)));

        public static readonly DependencyProperty DialogModelProperty = DependencyProperty.Register(
            nameof(DialogModel), typeof(object), typeof(DialogService), new PropertyMetadata(default(object)));

        private readonly IViewLocator _viewLocator;

        public DialogService(IViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
        }

        public Control DialogView
        {
            get => (Control)GetValue(DialogViewProperty);
            set => SetValue(DialogViewProperty, value);
        }

        public object DialogModel
        {
            get => GetValue(DialogModelProperty);
            set => SetValue(DialogModelProperty, value);
        }

        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            private set => SetValue(IsOpenProperty, value);
        }

        public IObservable<T> Show<T>(AbstractDialogModel<T> dialogModel)
        {
            var dialogViewName = dialogModel.GetType().Name.Replace("Model", string.Empty);
            return Show(dialogModel, dialogViewName);
        }

        public IObservable<T> Show<T>(AbstractDialogModel<T> dialogModel, string dialogViewName)
        {
            if (IsOpen)
                throw new InvalidOperationException("Dialog already open");

            DialogModel = dialogModel;
            DialogView = _viewLocator.ResolveView(dialogViewName);
            dialogModel.SetDialogService(this);
            IsOpen = true;

            return dialogModel;
        }

        public void CloseCurrentDialog()
        {
            IsOpen = false;
        }
    }
}