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
using Inventory.Data.Services;
using Inventory.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Inventory.ViewModels
{
    public class CreateDatabaseViewModel : ViewModelBase
    {
        public CreateDatabaseViewModel(ISettingsService settingsService, ICommonServices commonServices) : base(commonServices)
        {
            SettingsService = settingsService;
            Result = Result.Error("Operation cancelled");
        }

        public ISettingsService SettingsService { get; }

        public Result Result { get; private set; }

        private string _progressStatus = null;
        public string ProgressStatus
        {
            get => _progressStatus;
            set => Set(ref _progressStatus, value);
        }

        private double _progressMaximum = 1;
        public double ProgressMaximum
        {
            get => _progressMaximum;
            set => Set(ref _progressMaximum, value);
        }

        private double _progressValue = 0;
        public double ProgressValue
        {
            get => _progressValue;
            set => Set(ref _progressValue, value);
        }

        private string _message = null;
        public string Message
        {
            get { return _message; }
            set { if (Set(ref _message, value)) NotifyPropertyChanged(nameof(HasMessage)); }
        }

        public bool HasMessage => _message != null;

        private string _primaryButtonText;
        public string PrimaryButtonText
        {
            get => _primaryButtonText;
            set => Set(ref _primaryButtonText, value);
        }

        private string _secondaryButtonText = "Cancel";
        public string SecondaryButtonText
        {
            get => _secondaryButtonText;
            set => Set(ref _secondaryButtonText, value);
        }

        public async Task ExecuteAsync(string connectionString)
        {
            try
            {
                ProgressMaximum = 23;
                ProgressStatus = "Connecting to Database";
                using (var db = new SQLServerDb(connectionString))
                {
                    var dbCreator = db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                    if (!await dbCreator.ExistsAsync())
                    {
                        ProgressValue = 1;
                        ProgressStatus = "Creating Database...";
                        await db.Database.EnsureCreatedAsync();
                        ProgressValue = 2;
                        await CopyDataTables(db);
                        ProgressValue = 23;
                        Message = "Database created successfully.";
                        Result = Result.Ok("Database created successfully.");
                    }
                    else
                    {
                        ProgressValue = 23;
                        Message = $"Database already exists. Please, delete database and try again.";
                        Result = Result.Error("Database already exist");
                    }
                }
            }
            catch (Exception ex)
            {
                Result = Result.Error("Error creating database. See details in Activity Log");
                Message = $"Error creating database: {ex.Message}";
                LogException("Settings", "Create Database", ex);
            }
            PrimaryButtonText = "Ok";
            SecondaryButtonText = null;
        }

        private async Task CopyDataTables(SQLServerDb db)
        {
            using (var sourceDb = new SQLiteDb(SettingsService.PatternConnectionString))
            {
                #region Common

                ProgressStatus = "Создание таблицы Restaurants...";
                foreach (var item in sourceDb.Restaurants.AsNoTracking())
                {
                    await db.Restaurants.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 3;

                ProgressStatus = "Создание таблицы TaxTypes...";
                foreach (var item in sourceDb.TaxTypes.AsNoTracking())
                {
                    await db.TaxTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 4;

                ProgressStatus = "Создание таблицы Categories...";
                foreach (var item in sourceDb.Cities.AsNoTracking())
                {
                    await db.Cities.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 5;

                ProgressStatus = "Создание таблицы TypeStreet...";
                foreach (var item in sourceDb.StreetTypes.AsNoTracking())
                {
                    await db.StreetTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 6;

                ProgressStatus = "Создание таблицы Street...";
                foreach (var item in sourceDb.Streets.AsNoTracking())
                {
                    await db.Streets.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 7;

                /*ProgressStatus = "Создание таблицы Address...";
                foreach (var item in sourceDb.Address.AsNoTracking())
                {
                    await db.Address.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 8;*/

                ProgressStatus = "Создание таблицы Categories...";
                foreach (var item in sourceDb.MenuFolders.AsNoTracking())
                {
                    await db.MenuFolders.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 9;

                ProgressStatus = "Создание таблицы PaymentTypes...";
                foreach (var item in sourceDb.PaymentTypes.AsNoTracking())
                {
                    await db.PaymentTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 10;

                #endregion

                #region Products
                ProgressStatus = "Создание таблицы Products...";
                foreach (var item in sourceDb.Dishes.AsNoTracking())
                {
                    await db.Dishes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 11;
                #endregion

                #region Customer
                ProgressStatus = "Создание таблицы Customers...";
                foreach (var item in sourceDb.Customers.AsNoTracking())
                {
                    await db.Customers.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 12;
                #endregion

                #region Communications

                ProgressStatus = "Создание таблицы CommunicationTypes...";
                foreach (var item in sourceDb.CommunicationTypes.AsNoTracking())
                {
                    item.Id = 0;
                    await db.CommunicationTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 13;

                ProgressStatus = "Создание таблицы Communications...";
                foreach (var item in sourceDb.Communications.AsNoTracking())
                {
                    item.Id = 0;
                    await db.Communications.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 14;

                #endregion

                #region Order

                ProgressStatus = "Создание таблицы OrderSource...";
                foreach (var item in sourceDb.Sources.AsNoTracking())
                {
                    await db.Sources.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 15;

                ProgressStatus = "Создание таблицы OrderStatus...";
                foreach (var item in sourceDb.OrderStatuses.AsNoTracking())
                {
                    await db.OrderStatuses.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 16;

                ProgressStatus = "Создание таблицы OrderType...";
                foreach (var item in sourceDb.DeliveryTypes.AsNoTracking())
                {
                    await db.DeliveryTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 17;

                ProgressStatus = "Создание таблицы Order...";
                foreach (var item in sourceDb.Orders.AsNoTracking())
                {
                    await db.Orders.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 18;

                ProgressStatus = "Создание таблицы OrderItems...";
                foreach (var item in sourceDb.OrderDishes.AsNoTracking())
                {
                    await db.OrderDishes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 19;

                ProgressStatus = "Создание таблицы OrderStatusHistory...";
                foreach (var item in sourceDb.OrderStatusHistories.AsNoTracking())
                {
                    await db.OrderStatusHistories.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 20;

                ProgressStatus = "Создание таблицы OrderStatusSequence...";
                foreach (var item in sourceDb.OrderStatusSequences.AsNoTracking())
                {
                    item.Id = 0;
                    await db.OrderStatusSequences.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 21;

                #endregion

                #region DbVersion
                ProgressStatus = "Запись версии базы данных...";
                await db.DbVersion.AddAsync(await sourceDb.DbVersion.FirstAsync());
                await db.SaveChangesAsync();
                ProgressValue = 22;
                #endregion
            }
        }
    }
}
