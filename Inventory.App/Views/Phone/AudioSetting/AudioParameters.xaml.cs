using Inventory.Services;
using Windows.UI.Xaml.Controls;

namespace Inventory.Views
{
    public sealed partial class AudioParameters : UserControl
    {
        public AudioParametersViewModel ViewModel { get; set; }
        public AudioParameters()
        {
            ViewModel = ServiceLocator.Current.GetService<AudioParametersViewModel>();
            this.InitializeComponent();
        }
    }
}
