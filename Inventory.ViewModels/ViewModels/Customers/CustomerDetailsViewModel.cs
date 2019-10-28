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
using Inventory.Models.Enums;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region CustomerDetailsArgs
    public class CustomerDetailsArgs
    {
        static public CustomerDetailsArgs CreateDefault() => new CustomerDetailsArgs();

        public int CustomerId { get; set; }

        public bool IsNew => CustomerId <= 0;
    }
    #endregion

    public class CustomerDetailsViewModel : GenericDetailsViewModel<CustomerModel>
    {
        public CommunicationListViewModel CommunicationList { get; }

        public CustomerAddressListViewModel AddressList { get; }

        public CustomerDetailsViewModel(ICustomerService customerService, IVariableService variableService, 
            IFilePickerService filePickerService, ICommonServices commonServices) : base(commonServices)
        {
            CustomerService = customerService;
            VariableService = variableService;
            FilePickerService = filePickerService;

            CommunicationList = new CommunicationListViewModel(customerService, commonServices);
            AddressList = new CustomerAddressListViewModel(customerService, commonServices);
        }

        private string _phoneNumberIsRinging="";
        public string PhoneNumberIsRinging
        {
            get { return _phoneNumberIsRinging; }
            set { Set(ref _phoneNumberIsRinging, value); }
        }

        public ICustomerService CustomerService { get; }
        public IVariableService VariableService { get; }
        public IFilePickerService FilePickerService { get; }

        override public string Title => (Item?.IsNew ?? true) ? "Новый клиент" : TitleEdit;
        public string TitleEdit => Item == null ? "Клиент" : $"{Item.FullName}";

        public override bool ItemIsNew => Item?.IsNew ?? true;

        public CustomerDetailsArgs ViewModelArgs { get; private set; }

        public async Task LoadAsync(CustomerDetailsArgs args)
        {
            ViewModelArgs = args ?? CustomerDetailsArgs.CreateDefault();

            if (ViewModelArgs.IsNew)
            {
                Item = new CustomerModel
                {
                    RowGuid = Guid.NewGuid(),
                    SignOfConsent = await VariableService.GetVariableValueAsync<bool>(VariableStrings.SignOfConsentDefault),
                };
                IsEditMode = true;
            }
            else
            {
                try
                {
                    var item = await CustomerService.GetCustomerAsync(ViewModelArgs.CustomerId);
                    Item = item ?? new CustomerModel { Id = ViewModelArgs.CustomerId, RowGuid = Guid.NewGuid(), IsEmpty = true, SignOfConsent = true };
                    await PopulateCommunications(Item);
                    await PopulateAddresses(Item);
                    IsEditMode = false;
                }
                catch (Exception ex)
                {
                    LogException("Customer", "Load", ex);
                }
            }
        }
        public void Unload()
        {
            ViewModelArgs.CustomerId = Item?.Id ?? 0;
        }

        public void Subscribe()
        {
            MessageService.Subscribe<CustomerDetailsViewModel, CustomerModel>(this, OnDetailsMessage);
            MessageService.Subscribe<CustomerListViewModel>(this, OnListMessage);
        }
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        public CustomerDetailsArgs CreateArgs()
        {
            return new CustomerDetailsArgs
            {
                CustomerId = Item?.Id ?? 0
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

        protected override async Task<bool> SaveItemAsync(CustomerModel model)
        {
            try
            {
                StartStatusMessage("Saving customer...");
                await Task.Delay(100);
                await CustomerService.UpdateCustomerAsync(model);
                EndStatusMessage("Customer saved");
                LogInformation("Customer", "Save", "Customer saved successfully", $"Customer {model.Id} '{model.FullName}' was saved successfully.");
                if ((!string.IsNullOrEmpty(PhoneNumberIsRinging))&(ViewModelArgs.IsNew))
                {
                    CommunicationModel modelForPhone = new CommunicationModel();
                    modelForPhone.CustomerGuid = model.RowGuid;
                    modelForPhone.CustomerId = model.Id;
                    modelForPhone.TypeId = (int)CommunicationTypeEnum.Phone;
                    modelForPhone.Name = PhoneNumberIsRinging;
                    await CustomerService.UpdateCommunicationAsync(modelForPhone);
                }
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error saving Customer: {ex.Message}");
                LogException("Customer", "Save", ex);
                return false;
            }
        }

        protected override async void OnAfterSave()
        {
            await PopulateCommunications(Item);
            await PopulateAddresses(Item);
        }

        protected override async Task<bool> DeleteItemAsync(CustomerModel model)
        {
            try
            {
                StartStatusMessage("Deleting customer...");
                await Task.Delay(100);
                await CustomerService.DeleteCustomerAsync(model);
                EndStatusMessage("Customer deleted");
                LogWarning("Клиент", "Удаление", "Клиент удален", $"Клиент {model.Id} '{model.FullName}' был удален.");
                return true;
            }
            catch (Exception ex)
            {
                StatusError($"Error deleting Customer: {ex.Message}");
                LogException("Customer", "Delete", ex);
                return false;
            }
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Подтверждение удаления", "Вы уверены, что хотите удалить текущего клиента?", "Ok", "Отмена");
        }

        protected override IEnumerable<IValidationConstraint<CustomerModel>> GetValidationConstraints(CustomerModel model)
        {
            yield return new RequiredConstraint<CustomerModel>("Имя", m => m.FirstName);
        }

        /// <summary>
        /// Обработка внешних сообщений
        /// </summary>
        private async void OnDetailsMessage(CustomerDetailsViewModel sender, string message, CustomerModel changed)
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
                                    var item = await CustomerService.GetCustomerAsync(current.Id);
                                    item = item ?? new CustomerModel { Id = current.Id, IsEmpty = true };
                                    current.Merge(item);
                                    current.NotifyChanges();
                                    NotifyPropertyChanged(nameof(Title));
                                    if (IsEditMode)
                                    {
                                        StatusMessage("WARNING: This customer has been modified externally");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogException("Customer", "Handle Changes", ex);
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

        private async void OnListMessage(CustomerListViewModel sender, string message, object args)
        {
            var current = Item;
            if (current != null)
            {
                switch (message)
                {
                    case Messages.ItemsDeleted:
                        if (args is IList<CustomerModel> deletedModels)
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
                            var model = await CustomerService.GetCustomerAsync(current.Id);
                            if (model == null)
                            {
                                await OnItemDeletedExternally();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException("Customer", "Handle Ranges Deleted", ex);
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
                StatusMessage("WARNING: This customer has been deleted externally");
            });
        }

        private async Task PopulateCommunications(CustomerModel selectedItem)
        {
            try
            {
                if (selectedItem != null)
                {
                    await CommunicationList.LoadAsync(new CommunicationListArgs { CustomerId = selectedItem.Id }, silent: true);
                }
            }
            catch (Exception ex)
            {
                LogException("Customer", "Load communications", ex);
            }
        }

        private async Task PopulateAddresses(CustomerModel selectedItem)
        {
            try
            {
                if (selectedItem != null)
                {
                    await AddressList.LoadAsync(new CustomerAdddressListArgs { CustomerId = selectedItem.Id }, silent: true);
                }
            }
            catch (Exception ex)
            {
                LogException("Customer", "Load addresses", ex);
            }
        }
    }
}
