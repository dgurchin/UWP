using System.Windows.Input;

using Inventory.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Inventory.Views
{
    public sealed partial class OrderDishChoiceList : UserControl
    {
        public OrderDishChoiceList()
        {
            InitializeComponent();
        }

        #region ViewModel
        public DishListViewModel ViewModel
        {
            get { return (DishListViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(DishListViewModel), typeof(OrderDishChoiceList), new PropertyMetadata(null));
        #endregion

        #region NewCommand
        public ICommand NewCommand
        {
            get { return (ICommand)GetValue(NewCommandProperty); }
            set { SetValue(NewCommandProperty, value); }
        }
        public static readonly DependencyProperty NewCommandProperty = DependencyProperty.Register(nameof(NewCommand), typeof(ICommand), typeof(OrderDishChoiceList), new PropertyMetadata(null));
        #endregion
    }
}
