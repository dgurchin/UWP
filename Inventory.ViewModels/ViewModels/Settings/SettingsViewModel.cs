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

using System.Threading.Tasks;
using System.Windows.Input;

using Inventory.Common;
using Inventory.Services;

namespace Inventory.ViewModels
{
    #region SettingsArgs
    public class SettingsArgs
    {
        static public SettingsArgs CreateDefault() => new SettingsArgs();

        public bool DisableNavigationUntilSuccess { get; set; }
    }
    #endregion

    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(ISettingsService settingsService, ICommonServices commonServices) : base(commonServices)
        {
            SettingsService = settingsService;
        }

        public ISettingsService SettingsService { get; }

        public string Version => $"v{SettingsService.Version}";

        public bool IsEnabledTestMode
        {
            get
            {
                bool isDebugEnabled = false;
#if DEBUG
                isDebugEnabled = true;
#endif
                return isDebugEnabled;
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        private bool _isLocalProvider;
        public bool IsLocalProvider
        {
            get { return _isLocalProvider; }
            set { if (Set(ref _isLocalProvider, value)) UpdateProvider(); }
        }

        private bool _isSqlProvider;
        public bool IsSqlProvider
        {
            get => _isSqlProvider;
            set => Set(ref _isSqlProvider, value);
        }

        private string _sqlConnectionString = null;
        public string SqlConnectionString
        {
            get => _sqlConnectionString;
            set => Set(ref _sqlConnectionString, value);
        }

        public bool IsRandomErrorsEnabled
        {
            get { return SettingsService.IsRandomErrorsEnabled; }
            set { SettingsService.IsRandomErrorsEnabled = value; }
        }

        public string SipPhoneName
        {
            get { return SettingsService.SipPhoneName; }
            set { SettingsService.SipPhoneName = value; }
        }

        public string SipPhonePassword
        {
            get { return SettingsService.SipPhonePassword; }
            set { SettingsService.SipPhonePassword = value; }
        }

        public string SipPhoneDomain
        {
            get { return SettingsService.SipPhoneDomain; }
            set { SettingsService.SipPhoneDomain = value; }
        }

        public string SipPhoneTransportProtocol
        {
            get { return SettingsService.SipPhoneTransportProtocol; }
            set { SettingsService.SipPhoneTransportProtocol = value; }
        }
        public string SipPhonePort
        {
            get { return SettingsService.SipPhonePort; }
            set { SettingsService.SipPhonePort = value; }
        }


        private bool _isSipPhone;
        public bool IsSipPhone
        {
            get => _isSipPhone;
            set => Set(ref _isSipPhone, value);
        }

        public string MonitorHost
        {
            get { return SettingsService.MonitorHost; }
            set { SettingsService.MonitorHost = value; }
        }

        public string MonitorUserName
        {
            get { return SettingsService.MonitorUserName; }
            set { SettingsService.MonitorUserName = value; }
        }

        public string MonitorPassword
        {
            get { return SettingsService.MonitorPassword; }
            set { SettingsService.MonitorPassword = value; }
        }

        public ICommand ResetLocalDataCommand => new RelayCommand(OnResetLocalData);
        public ICommand ValidateSqlConnectionCommand => new RelayCommand(OnValidateSqlConnection);
        public ICommand CreateDatabaseCommand => new RelayCommand(OnCreateDatabase);
        public ICommand ApplyMigrationCommand => new RelayCommand(OnMigration);
        public ICommand CopyProductImagesCommand => new RelayCommand(OnCopyProductImages);
        public ICommand SaveChangesCommand => new RelayCommand(OnSaveChanges);
        public ICommand SipPhoneEnableCommand => new RelayCommand(OnSipPhoneСonnect);
        public ICommand SipPhoneDisconnectCommand => new RelayCommand(OnSipPhoneDisconnect);
        public ICommand SaveSipChangesCommand => new RelayCommand(OnSaveSipChanges);


        public SettingsArgs ViewModelArgs { get; private set; }

        public Task LoadAsync(SettingsArgs args)
        {
            ViewModelArgs = args ?? SettingsArgs.CreateDefault();

            StatusReady();

            IsLocalProvider = SettingsService.DataProvider == DataProviderType.SQLite;

            SqlConnectionString = SettingsService.SQLServerConnectionString;
            IsSqlProvider = SettingsService.DataProvider == DataProviderType.SQLServer;

            return Task.CompletedTask;
        }

        private void UpdateProvider()
        {
            if (IsLocalProvider && !IsSqlProvider)
            {
                SettingsService.DataProvider = DataProviderType.SQLite;
            }
        }

        private async void OnResetLocalData()
        {
            IsBusy = true;
            StatusMessage("Ожидание сброса базы данных...");
            var result = await SettingsService.ResetLocalDataProviderAsync();
            IsBusy = false;
            if (result.IsOk)
            {
                StatusReady();
            }
            else
            {
                StatusMessage(result.Message);
            }
        }

        private async void OnValidateSqlConnection()
        {
            await ValidateSqlConnectionAsync();
        }

        private async Task<bool> ValidateSqlConnectionAsync()
        {
            StatusReady();
            IsBusy = true;
            StatusMessage("Проверка строки подключения...");
            var result = await SettingsService.ValidateConnectionAsync(SqlConnectionString);
            IsBusy = false;
            if (result.IsOk)
            {
                StatusMessage(result.Message);
                return true;
            }
            else
            {
                StatusMessage(result.Message);
                return false;
            }
        }

        private async void OnCreateDatabase()
        {
            StatusReady();
            DisableAllViews("Ожидание сброса базы данных...");
            var result = await SettingsService.CreateDabaseAsync(SqlConnectionString);
            EnableOtherViews();
            EnableThisView("");
            await Task.Delay(100);
            if (result.IsOk)
            {
                StatusMessage(result.Message);
            }
            else
            {
                StatusError("Ошибка при создании базы данных");
            }
        }

        private async void OnMigration()
        {
            StatusReady();
            DisableAllViews("Ожидание применения миграции...");
            var result = await SettingsService.ApplyMigrationAsync(SqlConnectionString);
            EnableOtherViews();
            EnableThisView("");
            await Task.Delay(100);
            if (result.IsOk)
            {
                await LookupTablesProxy.Instance.InitializeAsync();
                StatusMessage(result.Message);
            }
            else
            {
                StatusError("Ошибка при применении миграции");
            }
        }

        private async void OnCopyProductImages()
        {
            StatusReady();
            DisableAllViews("Копирование изображений продуктов миграции...");
            var result = await SettingsService.CopyProductImagesAsync(SqlConnectionString);
            EnableOtherViews();
            EnableThisView("");
            await Task.Delay(100);
            if (result.IsOk)
            {
                StatusMessage(result.Message);
            }
            else
            {
                StatusError("Ошибка при копировании изображений");
            }
        }

        private async void OnSaveChanges()
        {
            if (IsSqlProvider)
            {
                if (await ValidateSqlConnectionAsync())
                {
                    SettingsService.SQLServerConnectionString = SqlConnectionString;
                    SettingsService.DataProvider = DataProviderType.SQLServer;
                    await LookupTablesProxy.Instance.InitializeAsync();
                }
            }
            else
            {
                SettingsService.DataProvider = DataProviderType.SQLite;
            }
        }

        //private async void OnSaveSipChanges()
        private void OnSaveSipChanges()
        {
            SettingsService.SipPhoneName = SipPhoneName;
            SettingsService.SipPhonePassword = SipPhonePassword;
            SettingsService.SipPhoneDomain = SipPhoneDomain;
            SettingsService.SipPhoneTransportProtocol = SipPhoneTransportProtocol;
            SettingsService.SipPhonePort = SipPhonePort;
        }

        //private async void OnSipPhoneСonnect()
        private void OnSipPhoneСonnect()
        {
            //StatusReady();
            //DisableAllViews("Ожидание подключения телефона ...");
            //await Task.Delay(100);
            IsSipPhone = true;
        }

        //private async void OnSipPhoneDisconnect()
        private void OnSipPhoneDisconnect()
        {
            //StatusReady();
            //DisableAllViews("Ожидание отключения телефона ...");
            //await Task.Delay(100);
            IsSipPhone = false;
        }
    }
}
