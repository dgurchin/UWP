using System;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrderDishChoiceDishesFilterViewModel : GenericListViewModel<MenuFolderModel>
    {
        public DishListViewModel Dishes { get; }

        public OrderDishChoiceDishesFilterViewModel(DishListViewModel dishes, ICommonServices commonServices) : base(commonServices)
        {
            Dishes = dishes;
            Items = LookupTables.MenuFolders;
            IsMultipleSelection = false;
            IsExtentedSelection = false;

            PropertyChanged += OrderDishChoiceDishesFilterViewModel_PropertyChanged;
        }

        protected override void OnDeleteSelection()
        {
        }

        protected override void OnNew()
        {
        }

        protected override void OnRefresh()
        {
        }

        private void OrderDishChoiceDishesFilterViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedItem))
            {
                FilterBySelectedItem(SelectedItem);
            }
        }

        private async void FilterBySelectedItem(MenuFolderModel model)
        {
            try
            {
                DishListArgs args1 = new DishListArgs();
                bool loadAll = model == null || model.RowGuid == Guid.Empty;
                if (loadAll)
                {
                    args1.OrderByDesc = r => r.CreatedOn;
                    await Dishes.LoadAsync(args1);
                }
                else
                {
                    args1.OrderByDesc = r => r.CreatedOn;
                    args1.Where = (dish) => dish.MenuFolderGuid == model.RowGuid;
                    await Dishes.LoadAsync(args1);
                }
            }
            catch (Exception ex)
            {
                LogException("Dishes", "Load Dishes by MenuFolder", ex);
            }
        }
    }
}
