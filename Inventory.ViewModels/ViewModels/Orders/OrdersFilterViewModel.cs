using System;
using System.Collections.Generic;
using System.Linq;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrdersFilterViewModel : GenericListViewModel<OrderStatusModel>
    {
        public OrderListViewModel OrderList { get; }

        public OrdersFilterViewModel(OrderListViewModel orderList, ICommonServices commonServices) : base(commonServices)
        {
            OrderList = orderList;
            Items = LookupTables.OrderStatuses;
            IsMultipleSelection = true;
            IsExtentedSelection = true;
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

        protected override void OnSelectItems(IList<object> items)
        {
            if (SelectedItems == null)
            {
                OnStartSelection();
            }
            base.OnSelectItems(items);
            FilterByStatuses(SelectedItems);
        }

        private async void FilterByStatuses(List<OrderStatusModel> orderStatuses)
        {
            var statusIds = orderStatuses.Select(x => x.Id).ToList();

            try
            {
                OrderListArgs args1 = new OrderListArgs();
                bool loadAll = statusIds.Count == 0 || statusIds.Any(x => x == 0);
                if (loadAll)
                {
                    args1.OrderByDesc = r => r.CreatedOn;
                    await OrderList.LoadAsync(args1);
                }
                else
                {
                    args1.OrderByDesc = r => r.CreatedOn;
                    args1.Where = (r) => statusIds.Contains(r.StatusId);
                    await OrderList.LoadAsync(args1);
                }
            }
            catch (Exception ex)
            {
                LogException("Orders", "Load StatusOrderItems", ex);
            }
        }
    }
}
