using System.Windows;
using System.Windows.Controls;

namespace DesktopApp.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogManagerView.xaml
    /// </summary>
    public partial class DialogBaseView : UserControl
    {
        public static readonly DependencyProperty DialogViewProperty =
            DependencyProperty.Register("DialogView", typeof(FrameworkElement), typeof(DialogBaseView),
                new PropertyMetadata(null));

        public DialogBaseView()
        {
            InitializeComponent();
        }

        public FrameworkElement DialogView
        {
            get => (FrameworkElement)GetValue(DialogViewProperty);
            set => SetValue(DialogViewProperty, value);
        }
    }
}