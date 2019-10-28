using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Inventory.Views
{
    public sealed partial class CommunicationList : UserControl
    {
        public CommunicationList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public CommunicationListViewModel ViewModel
        {
            get { return (CommunicationListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register("ViewModel", typeof(CommunicationListViewModel), typeof(CommunicationList), new PropertyMetadata(null));
        #endregion
    }
}
