using System.Windows.Controls;
using Prism.Regions;
using ReactiveUI;

namespace DesktopApp.Views;

public partial class StartPageView
{
    public StartPageView(IRegionManager regionManager)
    {
        InitializeComponent();

        this.WhenActivated(_ =>
        {
            var region = regionManager.Regions[RegionNames.ContentRegion];
            region.Activate(this);
        });
    }
}