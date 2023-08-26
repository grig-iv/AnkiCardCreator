using System;
using System.Reactive.Disposables;
using DesktopApp.ViewModels;
using Prism.Regions;
using ReactiveUI;

namespace DesktopApp.Views;

public partial class WordNotFoundPageView
{
    public WordNotFoundPageView(IRegionManager regionManager)
    {
        InitializeComponent();
        ViewModel = (WordNotFoundPageViewModel) DataContext;

        var region = regionManager.Regions[RegionNames.ContentRegion];
//        ViewModel.WhenPageLoaded.Subscribe(_ => region.Activate(this));

        this.WhenActivated(disposables =>
        {
            this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.Suggestions,
                    view => view.SuggestionList.ItemsSource)
                .DisposeWith(disposables);
        });
    }
}