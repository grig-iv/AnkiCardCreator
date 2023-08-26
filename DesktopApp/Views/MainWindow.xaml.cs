using Prism.Regions;

namespace DesktopApp.Views
{
    public partial class MainWindow 
    {
        public MainWindow(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion<HeaderView>(RegionNames.HeaderRegion);
            regionManager.RegisterViewWithRegion<StartPageView>(RegionNames.ContentRegion);
            regionManager.RegisterViewWithRegion<WordPageView>(RegionNames.ContentRegion);
            regionManager.RegisterViewWithRegion<WordNotFoundPageView>(RegionNames.ContentRegion);
        }
    }
}