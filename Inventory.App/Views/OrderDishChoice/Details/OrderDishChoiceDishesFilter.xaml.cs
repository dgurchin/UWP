using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Inventory.Views
{
    public sealed partial class OrderDishChoiceDishesFilter : UserControl
    {
        public OrderDishChoiceDishesFilter()
        {
            InitializeComponent();
        }

        #region ViewModel
        public OrderDishChoiceDishesFilterViewModel ViewModel
        {
            get { return (OrderDishChoiceDishesFilterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(OrderDishChoiceDishesFilterViewModel), typeof(OrderDishChoiceDishesFilter), new PropertyMetadata(null));
        #endregion
    }
}
