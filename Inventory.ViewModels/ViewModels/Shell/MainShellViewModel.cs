#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class MainShellViewModel : ShellViewModel
    {
        private readonly NavigationItem DashboardItem = new NavigationItem(0xE80F, "Информация", typeof(DashboardViewModel));
        private readonly NavigationItem CustomersItem = new NavigationItem(0xE716, "Клиенты", typeof(CustomersViewModel));
        private readonly NavigationItem OrdersItem = new NavigationItem(0xE8A1, "Заказы", typeof(OrdersViewModel));
        private readonly NavigationItem DishesItem = new NavigationItem(0xE781, "Меню", typeof(DishesViewModel));
        private readonly NavigationItem ChangesDatabaseItem = new NavigationItem(0xED5D, "Изм. БД SQLite", typeof(ChangesDatabaseDataViewModel));
        private readonly NavigationItem AppLogsItem = new NavigationItem(0xE7BA, "Журнал", typeof(AppLogsViewModel));
        private readonly NavigationItem SettingsItem = new NavigationItem(0x0000, "Настройки", typeof(SettingsViewModel));
        public MainShellViewModel(ILoginService loginService, ICommonServices commonServices) : base(loginService, commonServices)
        {
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private bool _isPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => Set(ref _isPaneOpen, value);
        }

        private IEnumerable<NavigationItem> _items;
        public IEnumerable<NavigationItem> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        public override async Task LoadAsync(ShellArgs args)
        {
            Items = GetItems().ToArray();
            await UpdateAppLogBadge();
            await base.LoadAsync(args);
        }

        override public void Subscribe()
        {
            MessageService.Subscribe<ILogService, AppLog>(this, OnLogServiceMessage);
            base.Subscribe();
        }

        override public void Unsubscribe()
        {
            base.Unsubscribe();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public async void NavigateTo(Type viewModel)
        {
            switch (viewModel.Name)
            {
                case nameof(DashboardViewModel):
                    NavigationService.Navigate(viewModel);
                    break;
                case nameof(CustomersViewModel):
                    NavigationService.Navigate(viewModel, new CustomerListArgs());
                    break;
                case nameof(OrdersViewModel):
                    NavigationService.Navigate(viewModel, new OrderListArgs());
                    break;
                case nameof(DishesViewModel):
                    NavigationService.Navigate(viewModel, new DishListArgs());
                    break;
                case nameof(ChangesDatabaseDataViewModel):
                    NavigationService.Navigate(viewModel, new ChangesDatabaseDataArgs());
                    break;
                case nameof(AppLogsViewModel):
                    NavigationService.Navigate(viewModel, new AppLogListArgs());
                    await LogService.MarkAllAsReadAsync();
                    await UpdateAppLogBadge();
                    break;
                case nameof(SettingsViewModel):
                    NavigationService.Navigate(viewModel, new SettingsArgs());
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return DashboardItem;
            yield return CustomersItem;
            yield return OrdersItem;
#if DEBUG
            yield return DishesItem;
            yield return ChangesDatabaseItem;
#endif
            yield return AppLogsItem;
        }

        private async void OnLogServiceMessage(ILogService logService, string message, AppLog log)
        {
            if (message == Messages.LogAdded)
            {
                await ContextService.RunAsync(async () =>
                {
                    await UpdateAppLogBadge();
                });
            }
        }

        private async Task UpdateAppLogBadge()
        {
            int count = await LogService.GetLogsCountAsync(new DataRequest<AppLog> { Where = r => !r.IsRead });
            AppLogsItem.Badge = count > 0 ? count.ToString() : null;
        }
    }
}
