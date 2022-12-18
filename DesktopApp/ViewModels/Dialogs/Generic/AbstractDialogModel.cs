using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using DesktopApp.Services;
using Reactive.Bindings;

namespace DesktopApp.ViewModels.Dialogs.Generic
{
    public abstract class AbstractDialogModel<T> : IObservable<T>, IDisposable
    {
        private readonly Subject<T> _dialogResultSubject;
        private readonly Subject<bool> _canAcceptProxy;
        private readonly Subject<bool> _canCancelProxy;
        private IDialogService? _dialogService;

        protected AbstractDialogModel(string header)
        {
            _dialogResultSubject = new Subject<T>();
            _canAcceptProxy = new Subject<bool>();
            _canCancelProxy = new Subject<bool>();

            Header = header;

            AcceptCaption = "ACCEPT";
            CancelCaption = "CANCEL";

            AcceptCommand = new ReactiveCommand(_canAcceptProxy);
            CancelCommand = new ReactiveCommand(_canCancelProxy);

            AcceptCommand.Subscribe(_ => _dialogResultSubject.OnNext(OnAccept()));

            WhenClosed.Subscribe(_ =>
            {
                _dialogService?.CloseCurrentDialog();
                _dialogResultSubject.OnCompleted();
                Dispose();
            });
        }

        public ReactiveCommand AcceptCommand { get; }
        public ReactiveCommand CancelCommand { get; }

        public string Header { get; protected set; }
        public bool IsOneButtonDialog { get; protected set; }

        public string AcceptCaption { get; protected set; }
        public string CancelCaption { get; protected set; }

        public IObservable<Unit> WhenClosed => Observable
            .Empty<Unit>()
            .Merge(AcceptCommand.Select(_ => Unit.Default))
            .Merge(CancelCommand.Select(_ => Unit.Default));

        public void SetDialogService(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        protected abstract T OnAccept();

        protected void SetCanAcceptSource(IObservable<bool> canAcceptSource)
        {
            var subscription = canAcceptSource.Subscribe(_canAcceptProxy);
            WhenClosed.Subscribe(_ => subscription.Dispose());
        }

        protected void SetCanCancelSource(IObservable<bool> canCancelSource)
        {
            var subscription = canCancelSource.Subscribe(_canCancelProxy);
            WhenClosed.Subscribe(_ => subscription.Dispose());
        }

        #region IObservable

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _dialogResultSubject.Subscribe(observer);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            _dialogResultSubject.Dispose();
            _canAcceptProxy.Dispose();
            _canCancelProxy.Dispose();
            AcceptCommand.Dispose();
            CancelCommand.Dispose();
        }

        #endregion
    }
}