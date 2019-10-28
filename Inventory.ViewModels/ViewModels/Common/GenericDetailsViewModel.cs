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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Common;
using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    abstract public partial class GenericDetailsViewModel<TModel> : ViewModelBase where TModel : ObservableObject, new()
    {
        public GenericDetailsViewModel(ICommonServices commonServices) : base(commonServices)
        {
        }

        public ILookupTables LookupTables => LookupTablesProxy.Instance;

        public bool IsDataAvailable => _item != null;
        public bool IsDataUnavailable => !IsDataAvailable;

        public bool CanGoBack => !IsMainView && NavigationService.CanGoBack;

        private TModel _prevItem;
        private TModel _item = null;
        public TModel Item
        {
            get => _item;
            set
            {
                if (Set(ref _item, value))
                {
                    if (_prevItem != null)
                    {
                        _prevItem.PropertyChanged -= Item_PropertyChanged;
                    }
                    if (_item != null)
                    {
                        _item.PropertyChanged += Item_PropertyChanged;
                    }
                    _prevItem = _item;
                    OnItemChanged(_item);

                    EditableItem = _item;
                    IsEnabled = (!_item?.IsEmpty) ?? false;
                    NotifyPropertyChanged(nameof(IsDataAvailable));
                    NotifyPropertyChanged(nameof(IsDataUnavailable));
                    NotifyPropertyChanged(nameof(Title));
                }
            }
        }

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnItemPropertyChanged(sender as TModel, e);
        }

        protected virtual void OnItemPropertyChanged(TModel item, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        protected virtual void OnItemChanged(TModel item)
        {
        }

        private TModel _prevEditableItem;
        private TModel _editableItem = null;
        public TModel EditableItem
        {
            get => _editableItem;
            set
            {
                if (Set(ref _editableItem, value))
                {
                    if (_prevEditableItem != null)
                    {
                        _prevEditableItem.PropertyChanged -= EditableItem_PropertyChanged;
                    }
                    if (_editableItem != null)
                    {
                        _editableItem.PropertyChanged += EditableItem_PropertyChanged;
                    }
                    _prevEditableItem = _editableItem;

                    OnEditableItemChanged(_editableItem);
                }
            }
        }

        private void EditableItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnEditableItemPropertyChanged(sender as TModel, e);
        }

        protected virtual void OnEditableItemPropertyChanged(TModel editableItem, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        protected virtual void OnEditableItemChanged(TModel editableItem)
        {
        }

        private bool _isEditMode = false;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => Set(ref _isEditMode, value);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(ref _isEnabled, value);
        }

        public ICommand BackCommand => new RelayCommand(OnBack);
        virtual protected void OnBack()
        {
            StatusReady();
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        public ICommand EditCommand => new RelayCommand(OnEdit);
        virtual protected void OnEdit()
        {
            StatusReady();
            BeginEdit();
            MessageService.Send(this, Messages.BeginEdit, Item);
        }
        virtual public void BeginEdit()
        {
            if (!IsEditMode)
            {
                IsEditMode = true;
                // Create a copy for edit
                var editableItem = new TModel();
                OnBeforeMergeEditable(Item, editableItem);
                editableItem.Merge(Item);
                EditableItem = editableItem;
            }
        }

        public ICommand CancelCommand => new RelayCommand(OnCancel);
        virtual protected void OnCancel()
        {
            StatusReady();
            CancelEdit();
            MessageService.Send(this, Messages.CancelEdit, Item);
        }
        virtual public void CancelEdit()
        {
            if (ItemIsNew)
            {
                // We were creating a new item: cancel means exit
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    NavigationService.CloseCurrentView();
                }
                return;
            }

            // We were editing an existing item: just cancel edition
            if (IsEditMode)
            {
                EditableItem = Item;
            }
            IsEditMode = false;
        }

        public DetailSaveBehaviour SaveBehaviour { get; set; } = DetailSaveBehaviour.SaveAndKeep;

        public ICommand SaveCommand => new RelayCommand(OnSave);
        virtual protected async void OnSave()
        {
            StatusReady();
            var result = Validate(EditableItem);
            if (result.IsOk)
            {
                await SaveAsync();
                if (SaveBehaviour == DetailSaveBehaviour.SaveAndClose)
                {
                    NavigationService.CloseCurrentView();
                }
            }
            else
            {
                await DialogService.ShowAsync(result.Message, $"{result.Description} Пожалуйста, исправьте ошибку и повторите снова.");
            }
        }
        virtual public async Task SaveAsync()
        {
            IsEnabled = false;
            bool isNew = ItemIsNew;
            bool canSave = await OnBeforeSaveAsync(EditableItem);
            if (canSave && await SaveItemAsync(EditableItem))
            {
                OnBeforeMerge(Item, EditableItem);
                Item.Merge(EditableItem);
                EditableItem = Item;
                OnAfterSave();
                Item.NotifyChanges();
                NotifyPropertyChanged(nameof(Title));

                if (isNew)
                {
                    MessageService.Send(this, Messages.NewItemSaved, Item);
                }
                else
                {
                    MessageService.Send(this, Messages.ItemChanged, Item);
                }
                IsEditMode = false;

                NotifyPropertyChanged(nameof(ItemIsNew));
            }
            IsEnabled = true;
        }

        protected virtual void OnBeforeMerge(TModel item, TModel editableItem)
        {
        }

        protected virtual void OnBeforeMergeEditable(TModel item, TModel editableItem)
        {
        }

        protected virtual async Task<bool> OnBeforeSaveAsync(TModel editableModel)
        {
            return await Task.FromResult(true);
        }

        protected virtual void OnAfterSave()
        {
        }

        public ICommand DeleteCommand => new RelayCommand(OnDelete);
        virtual protected async void OnDelete()
        {
            StatusReady();
            if (await ConfirmDeleteAsync())
            {
                await DeleteAsync();
            }
        }
        virtual public async Task DeleteAsync()
        {
            var model = Item;
            if (model != null)
            {
                IsEnabled = false;
                if (await DeleteItemAsync(model))
                {
                    MessageService.Send(this, Messages.ItemDeleted, model);
                }
                else
                {
                    IsEnabled = true;
                }
            }
        }

        virtual public Result Validate(TModel editableModel)
        {
            foreach (var constraint in GetValidationConstraints(editableModel))
            {
                if (!constraint.Validate(editableModel))
                {
                    return Result.Error("Ошибка валидации", constraint.Message);
                }
            }
            return Result.Ok();
        }

        virtual protected IEnumerable<IValidationConstraint<TModel>> GetValidationConstraints(TModel editableModel) => Enumerable.Empty<IValidationConstraint<TModel>>();

        abstract public bool ItemIsNew { get; }

        abstract protected Task<bool> SaveItemAsync(TModel model);
        abstract protected Task<bool> DeleteItemAsync(TModel model);
        abstract protected Task<bool> ConfirmDeleteAsync();
    }
}
