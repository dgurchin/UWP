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
using System.Threading.Tasks;

using Inventory.Common;
using Inventory.Views;

using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace Inventory.Services
{
    public class SettingsService : ISettingsService
    {
        public SettingsService(IDialogService dialogService)
        {
            DialogService = dialogService;
        }

        public IDialogService DialogService { get; }

        public string Version => AppSettings.Current.Version;

        public string DbVersion => AppSettings.Current.DbVersion;

        public string UserName
        {
            get => AppSettings.Current.UserName;
            set => AppSettings.Current.UserName = value;
        }

        public DataProviderType DataProvider
        {
            get => AppSettings.Current.DataProvider;
            set => AppSettings.Current.DataProvider = value;
        }

        public string PatternConnectionString => $"Data Source={AppSettings.DatabasePatternFileName}";

        public string SQLServerConnectionString
        {
            get => AppSettings.Current.SQLServerConnectionString;
            set => AppSettings.Current.SQLServerConnectionString = value;
        }

        public string SQLiteConnectionString
        {
            get => AppSettings.Current.SQLiteConnectionString;
        }

        public bool IsRandomErrorsEnabled
        {
            get => AppSettings.Current.IsRandomErrorsEnabled;
            set => AppSettings.Current.IsRandomErrorsEnabled = value;
        }

        public string SipPhoneName
        {
            get => AppSettings.Current.SipPhoneName;
            set => AppSettings.Current.SipPhoneName = value;
        }

        public string SipPhonePassword
        {
            get => AppSettings.Current.SipPhonePassword;
            set => AppSettings.Current.SipPhonePassword = value;
        }

        public string SipPhoneDomain
        {
            get => AppSettings.Current.SipPhoneDomain;
            set => AppSettings.Current.SipPhoneDomain = value;
        }

        public string SipPhoneTransportProtocol
        {
            get => AppSettings.Current.SipPhoneTransportProtocol;
            set => AppSettings.Current.SipPhoneTransportProtocol = value;
        }

        public string SipPhonePort
        {
            get => AppSettings.Current.SipPhonePort;
            set => AppSettings.Current.SipPhonePort = value;
        }

        public string MonitorHost { get => AppSettings.Current.MonitorHost; set => AppSettings.Current.MonitorHost = value; }
        public string MonitorUserName { get => AppSettings.Current.MonitorUserName; set => AppSettings.Current.MonitorUserName = value; }
        public string MonitorPassword { get => AppSettings.Current.MonitorPassword; set => AppSettings.Current.MonitorPassword = value; }

        public async Task<Result> ResetLocalDataProviderAsync()
        {
            Result result = null;
            string errorMessage = null;
            try
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var databaseFolder = await localFolder.CreateFolderAsync(AppSettings.DatabasePath, CreationCollisionOption.OpenIfExists);
                var sourceFile = await databaseFolder.GetFileAsync(AppSettings.DatabasePattern);
                var targetFile = await databaseFolder.CreateFileAsync(AppSettings.DatabaseName, CreationCollisionOption.ReplaceExisting);
                await sourceFile.CopyAndReplaceAsync(targetFile);
                await DialogService.ShowAsync("Reset Local Data Provider", "Local Data Provider restore successfully.");
                result = Result.Ok();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                result = Result.Error(ex);
            }
            if (errorMessage != null)
            {
                await DialogService.ShowAsync("Reset Local Data Provider", errorMessage);
            }
            return result;
        }

        public async Task<Result> ValidateConnectionAsync(string connectionString)
        {
            var dialog = new ValidateConnectionView(connectionString);
            var res = await dialog.ShowAsync();
            switch (res)
            {
                case ContentDialogResult.Secondary:
                    return Result.Ok("Operation canceled by user");
                default:
                    break;
            }
            return dialog.Result;
        }

        public async Task<Result> CreateDabaseAsync(string connectionString)
        {
            var dialog = new CreateDatabaseView(connectionString);
            var res = await dialog.ShowAsync();
            switch (res)
            {
                case ContentDialogResult.Secondary:
                    return Result.Ok("Operation canceled by user");
                default:
                    break;
            }
            return dialog.Result;
        }

        public async Task<Result> ApplyMigrationAsync(string connectionString)
        {
            var dialog = new MigrationView(connectionString);
            var res = await dialog.ShowAsync();
            switch (res)
            {
                case ContentDialogResult.Secondary:
                    return Result.Ok("Operation canceled by user");
                default:
                    break;
            }
            return dialog.Result;
        }

        public async Task<Result> CopyProductImagesAsync(string connectionString)
        {
            var dialog = new MigrationView(connectionString, true);
            var res = await dialog.ShowAsync();
            switch (res)
            {
                case ContentDialogResult.Secondary:
                    return Result.Ok("Operation canceled by user");
                default:
                    break;
            }
            return dialog.Result;
        }
    }
}
