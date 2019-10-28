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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Common;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region DishDetailsArgs
    public class DishDetailsArgs
    {
        static public DishDetailsArgs CreateDefault() => new DishDetailsArgs();

        public int DishId { get; set; }

        public bool IsNew => DishId <= 0;
    }
    #endregion

    public class DishDetailsViewModel : GenericDetailsViewModel<DishModel>
    {
        public DishDetailsViewModel(IDishService dishService, IFilePickerService filePickerService, ICommonServices commonServices) : base(commonServices)
        {
            DishService = dishService;
            FilePickerService = filePickerService;
        }

        public IDishService DishService { get; }
        public IFilePickerService FilePickerService { get; }

        override public string Title => (Item?.IsNew ?? true) ? "Новое блюдо" : TitleEdit;
        public string TitleEdit => Item == null ? "Блюдо" : $"{Item.Name}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        public DishDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(DishDetailsArgs args)
        {
            ViewModelArgs = args ?? DishDetailsArgs.CreateDefault();

            if (ViewModelArgs.IsNew)
            {
                Item = new DishModel();
                IsEditMode = true;
            }
            else
            {
                try
                {
                    var item = await DishService.GetDishAsync(ViewModelArgs.DishId);
                    Item = item ?? new DishModel { Id = ViewModelArgs.DishId, IsEmpty = true };
                }
                catch (Exception ex)
                {
                    LogException("Dish", "Load", ex);
                }
            }
        }
        public void Unload()
        {
            ViewModelArgs.DishId = Item.Id;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<DishDetailsViewModel, DishModel>(this, OnDetailsMessage);
            MessageService.Subscribe<DishListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public DishDetailsArgs CreateArgs()
        {
            return new DishDetailsArgs
            {
                DishId = Item.Id
            };
        }

        private object _newPictureSource = null;
        public object NewPictureSource
        {
            get => _newPictureSource;
            set => Set(ref _newPictureSource, value);
        }

        public override void BeginEdit()
        {
            NewPictureSource = null;
            base.BeginEdit();
        }

        public ICommand EditPictureCommand => new RelayCommand(OnEditPicture);
        private async void OnEditPicture()
        {
            NewPictureSource = null;
            var result = await FilePickerService.OpenImagePickerAsync();
            if (result != null)
            {
                EditableItem.Picture = result.ImageBytes;
                EditableItem.PictureSource = result.ImageSource;
                EditableItem.Thumbnail = result.ImageBytes;
                EditableItem.ThumbnailSource = result.ImageSource;
                NewPictureSource = result.ImageSource;
            }
            else
            {
                NewPictureSource = null;
            }
        }

        protected override async Task<bool> SaveItemAsync(DishModel model)
        {
            try
            {
                StartStatusMessage("Сохранение блюда...");
                await Task.Delay(100);
                await DishService.UpdateDishAsync(model);
                EndStatusMessage("Блюдо сохранено");
                LogInformation("Dish", "Save", "Блюдо сохранено успешно", $"Блюдо {model.Id} '{model.Name}' сохранено успешно.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Ошибка сохранения блюда: {ex.Message}");
                LogException("Dish", "Save", ex);
                return false;
            }
        }

        protected override async Task<bool> DeleteItemAsync(DishModel model)
        {
            try
            {
                StartStatusMessage("Блюдо удаляется...");
                await Task.Delay(100);
                await DishService.DeleteDishAsync(model);
                EndStatusMessage("Блюдо удалено");
                LogWarning("Dish", "Delete", "Блюдо удалено", $"Блюдо {model.Id} '{model.Name}' удалено.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Ошибка удаления блюда: {ex.Message}");
                LogException("Dish", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Подтверждение удаления", "Вы уверены, что хотите удалить это блюдо?", "Ok", "Отмена");
        }

        override protected IEnumerable<IValidationConstraint<DishModel>> GetValidationConstraints(DishModel model)
        {
            yield return new RequiredConstraint<DishModel>("Name", m => m.Name);
            yield return new RequiredGreaterThanZeroConstraint<DishModel>("Category", m => m.MenuFolderGuid);
        }

        /*
         *  Handle external messages
         ****************************************************************/
        private async void OnDetailsMessage(DishDetailsViewModel sender, string message, DishModel changed)
        {
            var current = Item;
            if (current != null)
            {
                if (changed != null && changed.Id == current?.Id)
                {
                    switch (message)
                    {
                        case Messages.ItemChanged:
                            await ContextService.RunAsync(async () =>
                            {
                                try
                                {
                                    var item = await DishService.GetDishAsync(current.Id);
                                    item = item ?? new DishModel { Id = current.Id, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("ПРЕДУПРЕЖДЕНИЕ: Это блюдо изменено извне");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Dish", "Handle Changes", ex);
                                }
                            });
                            break;
                        case Messages.ItemDeleted:
                            await OnItemDeletedExternally();
                            break;
                    }
                }
            }
        }

        private async void OnListMessage(DishListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case Messages.ItemsDeleted:
                        if (args is IList<DishModel> deletedModels)
                        {
                            if (deletedModels.Any(r => r.Id == current.Id))
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        break;
                    case Messages.ItemRangesDeleted:
                        try
                        {
                            var model = await DishService.GetDishAsync(current.Id);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Dish", "Handle Ranges Deleted", ex);
                        }
                        break;
                }
            }
        }

        private async Task OnItemDeletedExternally()
        {
            await ContextService.RunAsync(() =>
            {
                CancelEdit();
                IsEnabled = false;
                StatusMessage("ПРЕДУПРЕЖДЕНИЕ: Это блюдо изменено извне");
            });
        }
    }
}
