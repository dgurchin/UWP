using Inventory.Services;
using Windows.UI.Xaml.Controls;

namespace Inventory.Views
{
    public sealed partial class AudioCodecs : UserControl
    {
        public AudioCodecsViewModel ViewModel { get; set; }
        public AudioCodecs()
        {
            ViewModel = ServiceLocator.Current.GetService<AudioCodecsViewModel>();
            this.InitializeComponent();
        }
    }
}

