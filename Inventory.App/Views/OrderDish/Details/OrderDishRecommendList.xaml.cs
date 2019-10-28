using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Inventory.Views
{
    public sealed partial class OrderDishRecommendList : UserControl
    {
        public OrderDishRecommendList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderDishRecommendViewModel ViewModel
        {
            get { return (OrderDishRecommendViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderDishRecommendViewModel), typeof(OrderDishRecommendList), new PropertyMetadata(null));
        #endregion
    }
}
