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
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;
using Inventory.Services;
using Inventory.Common;

namespace Inventory.ViewModels
{
    #region AppLogListArgs
    public class AppLogListArgs
    {
        static public AppLogListArgs CreateEmpty() => new AppLogListArgs { IsEmpty = true };

        public AppLogListArgs()
        {
            OrderByDesc = r => r.DateTime;
        }

        public bool IsEmpty { get; set; }

        public string Query { get; set; }

        public Expression<Func<AppLog, object>> OrderBy { get; set; }
        public Expression<Func<AppLog, object>> OrderByDesc { get; set; }
    }
    #endregion

    public class AppLogListViewModel : GenericListViewModel<AppLogModel>
    {
        public AppLogListViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        public AppLogListArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(AppLogListArgs args)
        {
            ViewModelArgs = args ?? AppLogListArgs.CreateEmpty();
            Query = ViewModelArgs.Query;

            StartStatusMessage("Загрузка записей...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Записи загружены");
            }
        }
        public void Unload()
        {
            ViewModelArgs.Query = Query;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<AppLogListViewModel>(this, OnMessage);
            MessageService.Subscribe<AppLogDetailsViewModel>(this, OnMessage);
            MessageService.Subscribe<ILogService, AppLog>(this, OnLogServiceMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public AppLogListArgs CreateArgs()
        {
            return new AppLogListArgs
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Items = null;
            ItemsCount = 0;
            SelectedItem = null;

            try
            {
                Items = await GetItemsAsync();
            }
            catch (Exception ex)
            {
                Items = new List<AppLogModel>();
                StatusError($"Ошибка загрузки записей журнала: {ex.Message}");
                LogException("AppLogs", "Refresh", ex);
                isOk = false;
            }

            ItemsCount = Items.Count;
            if (!IsMultipleSelection)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            NotifyPropertyChanged(nameof(Title));

            return isOk;
        }

        private async Task<IList<AppLogModel>> GetItemsAsync()
        {
            if (!ViewModelArgs.IsEmpty)
            {
                DataRequest<AppLog> request = BuildDataRequest();
                return await LogService.GetLogsAsync(request);
            }
            return new List<AppLogModel>();
        }

        protected override void OnNew()
        {
            throw new NotImplementedException();
        }

        protected override async void OnRefresh()
        {
            StartStatusMessage("Записи журнала загружаются...");
            if (await RefreshAsync())
            {
                EndStatusMessage("Записи журнала загружены");
            }
        }

        protected override async void OnDeleteSelection()
        {
            StatusReady();
            if (await DialogService.ShowAsync("Подтверждение удаления", "Вы уверены, что хотите удалить выделенную запись журнала?", "Ok", "Отменить"))
            {
                int count = 0;
                try
                {
                    if (SelectedIndexRanges != null)
                    {
                        count = SelectedIndexRanges.Sum(r => r.Length);
                        StartStatusMessage($"Удаляется {count} записей...");
                        await DeleteRangesAsync(SelectedIndexRanges);
                        MessageService.Send(this, Messages.ItemRangesDeleted, SelectedIndexRanges);
                    }
                    else if (SelectedItems != null)
                    {
                        count = SelectedItems.Count();
                        StartStatusMessage($"Удаляется {count} записей...");
                        await DeleteItemsAsync(SelectedItems);
                        MessageService.Send(this, Messages.ItemsDeleted, SelectedItems);
                    }
                }
                catch (Exception ex)
                {
                    StatusError($"Ошибка удаления {count} записей: {ex.Message}");
                    LogException("AppLogs", "Delete", ex);
                    count = 0;
                }
                await RefreshAsync();
                SelectedIndexRanges = null;
                SelectedItems = null;
                if (count > 0)
                {
                    EndStatusMessage($"{count} Записей удалено");
                }
            }
        }

        private async Task DeleteItemsAsync(IEnumerable<AppLogModel> models)
        {
            foreach (var model in models)
            {
                await LogService.DeleteLogAsync(model);
            }
        }

        private async Task DeleteRangesAsync(IEnumerable<IndexRange> ranges)
        {
            DataRequest<AppLog> request = BuildDataRequest();
            foreach (var range in ranges)
            {
                await LogService.DeleteLogRangeAsync(range.Index, range.Length, request);
            }
        }

        private DataRequest<AppLog> BuildDataRequest()
        {
            return new DataRequest<AppLog>()
            {
                Query = Query,
                OrderBy = ViewModelArgs.OrderBy,
                OrderByDesc = ViewModelArgs.OrderByDesc
            };
        }

        private async void OnMessage(ViewModelBase sender, string message, object args)
        {
            switch (message)
            {
                case Messages.NewItemSaved:
                case Messages.ItemDeleted:
                case Messages.ItemsDeleted:
                case Messages.ItemRangesDeleted:
                    await ContextService.RunAsync(async () =>
                    {
                        await RefreshAsync();
                    });
                    break;
            }
        }

        private async void OnLogServiceMessage(ILogService logService, string message, AppLog log)
        {
            if (message == Messages.LogAdded)
            {
                await ContextService.RunAsync(async () =>
                {
                    await RefreshAsync();
                });
            }
        }
    }
}
