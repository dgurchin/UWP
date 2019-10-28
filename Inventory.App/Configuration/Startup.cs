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

using Inventory.Services;
using Inventory.ViewModels;
using Inventory.Views;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

using Microsoft.Extensions.DependencyInjection;

using Windows.Foundation;
using Windows.Storage;
using Windows.UI.ViewManagement;

namespace Inventory
{
    static public class Startup
    {
        static private readonly ServiceCollection _serviceCollection = new ServiceCollection();

        static public async Task ConfigureAsync()
        {
            AppCenter.Start("75aa8f17-39c4-4648-a607-5e9176381410", typeof(Analytics));
            //AppCenter.Start("75aa8f17-39c4-4648-a607-5e9176381410", typeof(Analytics), typeof(Crashes));
            Analytics.TrackEvent("AppStarted");

            ServiceLocator.Configure(_serviceCollection);

            ConfigureNavigation();

            await EnsureLogDbAsync();
            await ConfigureLookupTables();

            var logService = ServiceLocator.Current.GetService<ILogService>();
            await logService.WriteAsync(Data.LogType.Information, "Startup", "Конфигурация", "Старт Приложения", $"Приложение стартовало.");

            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }

        private static void ConfigureNavigation()
        {
            NavigationService.Register<LoginViewModel, LoginView>();

            NavigationService.Register<ShellViewModel, ShellView>();
            NavigationService.Register<MainShellViewModel, MainShellView>();

            NavigationService.Register<DashboardViewModel, DashboardView>();

            NavigationService.Register<CustomersViewModel, CustomersView>();
            NavigationService.Register<CustomerDetailsViewModel, CustomerView>();

            NavigationService.Register<OrdersViewModel, OrdersView>();
            NavigationService.Register<OrderDetailsViewModel, OrderView>();

            NavigationService.Register<OrderDishesViewModel, OrderDishesView>();
            NavigationService.Register<OrderDishDetailsViewModel, OrderDishView>();
            NavigationService.Register<OrderDishChoiceViewModel, OrderDishChoiceView>();

            NavigationService.Register<DishesViewModel, DishesView>();
            NavigationService.Register<DishDetailsViewModel, DishView>();

            NavigationService.Register<ChangesDatabaseDataViewModel, ChangesDatabaseDataView>();

            NavigationService.Register<AppLogsViewModel, AppLogsView>();

            NavigationService.Register<SettingsViewModel, SettingsView>();

            //NavigationService.Register<PhoneCallViewModel, PhoneCallView>();
            NavigationService.Register<PhoneCallReceiveViewModel, PhoneCallReceiveView>();
        }

        static private async Task EnsureLogDbAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var appLogFolder = await localFolder.CreateFolderAsync(AppSettings.AppLogPath, CreationCollisionOption.OpenIfExists);
            if (await appLogFolder.TryGetItemAsync(AppSettings.AppLogName) == null)
            {
                var sourceLogFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/AppLog/AppLog.db"));
                var targetLogFile = await appLogFolder.CreateFileAsync(AppSettings.AppLogName, CreationCollisionOption.ReplaceExisting);
                await sourceLogFile.CopyAndReplaceAsync(targetLogFile);
            }
        }

        static private async Task EnsureDatabaseAsync()
        {
            // Всегда копировать базу данных
            await EnsureSQLiteDatabaseAsync();
        }

        /// <summary>
        /// Копирование тестовой БД
        /// </summary>
        /// <returns></returns>
        private static async Task EnsureSQLiteDatabaseAsync()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var databaseFolder = await localFolder.CreateFolderAsync(AppSettings.DatabasePath, CreationCollisionOption.OpenIfExists);

            if (await databaseFolder.TryGetItemAsync(AppSettings.DatabaseName) == null)
            {
                if (await databaseFolder.TryGetItemAsync(AppSettings.DatabasePattern) == null)
                {
                    var sourceFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/SQLiteBase/Delivery.2.02.db"));
                    var file = await databaseFolder.CreateFileAsync(AppSettings.DatabasePattern, CreationCollisionOption.ReplaceExisting);
                    await sourceFile.CopyAndReplaceAsync(file);
                }
            }
            var sourcePatternFile = await databaseFolder.GetFileAsync(AppSettings.DatabasePattern);
            var targetPatternFile = await databaseFolder.CreateFileAsync(AppSettings.DatabaseName, CreationCollisionOption.ReplaceExisting);
            await sourcePatternFile.CopyAndReplaceAsync(targetPatternFile);
        }

        static private async Task ConfigureLookupTables()
        {
            var lookupTables = ServiceLocator.Current.GetService<ILookupTables>();
            await lookupTables.InitializeAsync();
            LookupTablesProxy.Instance = lookupTables;
        }

        public static async Task<bool> ValidateConnectionAsync(bool silent = false)
        {
            var logService = ServiceLocator.Current.GetService<ILogService>();
            var settings = ServiceLocator.Current.GetService<ISettingsService>();
            var viewModel = ServiceLocator.Current.GetService<ValidateConnectionViewModel>();

            bool result;
            try
            {
                await viewModel.ExecuteAsync(settings.SQLServerConnectionString);
                result = viewModel.Result.IsOk;
                if (!result && !silent)
                {
                    await logService.WriteAsync(Data.LogType.Warning, "Startup", "ValidateConnection", viewModel.Message, string.Empty);
                }
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                if (!silent)
                {
                    await logService.WriteAsync(Data.LogType.Error, "Startup", "ValidateConnection", ex);
                }
                return result;
            }
        }
    }
}
