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
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    public class SQLServerDb : DbContext, IDataSource
    {
        #region Private fields
        private readonly string _connectionString = null;
        #endregion

        #region Tables
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Street> Streets { get; set; }
        public DbSet<StreetType> StreetTypes { get; set; }

        public DbSet<Communication> Communications { get; set; }
        public DbSet<CommunicationType> CommunicationTypes { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<DbVersion> DbVersion { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }

        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishGarnish> DishGarnishes { get; set; }
        public DbSet<DishIngredient> DishIngredients { get; set; }
        public DbSet<DishMark> DishMarks { get; set; }
        public DbSet<DishModifier> DishModifiers { get; set; }
        public DbSet<DishRecommend> DishRecommends { get; set; }
        public DbSet<DishUnit> DishUnits { get; set; }

        public DbSet<Mark> Marks { get; set; }
        public DbSet<Modifier> Modifiers { get; set; }

        public DbSet<MenuFolder> MenuFolders { get; set; }
        public DbSet<MenuFolderModifier> MenuFolderModifiers { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDish> OrderDishes { get; set; }
        public DbSet<OrderDishGarnish> OrderDishGarnishes { get; set; }
        public DbSet<OrderDishIngredient> OrderDishIngredients { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        public DbSet<OrderStatusSequence> OrderStatusSequences { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<TaxType> TaxTypes { get; set; }
        public DbSet<Variable> Variables { get; set; }

        public DbSet<StopSheet> StopSheets { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<CourierOrder> CourierOrders { get; set; }
        #endregion

        #region Ctor
        public SQLServerDb(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Protected methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.OnDeleteRestrict<Address, City>(x => x.City, x => x.Addresses);
            modelBuilder.OnDeleteRestrict<Address, Street>(x => x.Street, x => x.Addresses);
            modelBuilder.OnDeleteRestrict<Street, StreetType>(x => x.StreetType, x => x.Streets);
            modelBuilder.OnDeleteRestrict<Street, City>(x => x.City, x => x.Streets);

            modelBuilder.OnDeleteRestrict<Communication, CommunicationType>(x => x.Type, x => x.Communications);

            modelBuilder.Entity<Customer>().Property(x => x.SignOfConsent).HasDefaultValue(true);
            modelBuilder.OnDeleteRestrict<Customer, Source>(x => x.Source, x => x.Customers);

            modelBuilder.Entity<Dish>().HasOne(x => x.MenuFolder).WithMany(menuFolder => menuFolder.Dishes)
                .HasForeignKey(dish => dish.MenuFolderGuid)
                .HasPrincipalKey(menuFolder => menuFolder.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Dish>().HasOne(x => x.DishUnit).WithMany(dishUnit => dishUnit.Dishes)
                .HasForeignKey(dish => dish.DishUnitGuid)
                .HasPrincipalKey(dishUnit => dishUnit.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Dish>().HasOne(x => x.TaxType).WithMany(taxType => taxType.Dishes)
                .HasForeignKey(dish => dish.TaxTypeGuid)
                .HasPrincipalKey(taxType => taxType.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DishGarnish>().HasOne(x => x.Dish).WithMany(dish => dish.DishGarnishes)
                .HasForeignKey(garnish => garnish.DishGuid)
                .HasPrincipalKey(dish => dish.RowGuid);

            modelBuilder.Entity<DishIngredient>().HasOne(x => x.Dish).WithMany(dish => dish.DishIngredients)
                .HasForeignKey(ingredient => ingredient.DishGuid)
                .HasPrincipalKey(dish => dish.RowGuid);

            modelBuilder.Entity<DishRecommend>().HasOne(x => x.Dish).WithMany(dish => dish.DishRecommends)
                .HasForeignKey(recommend => recommend.DishGuid)
                .HasPrincipalKey(dish => dish.RowGuid);
            modelBuilder.Entity<DishRecommend>().HasOne(x => x.RecommendDish).WithMany(dish => dish.RecommendDishes)
                .HasForeignKey(recommend => recommend.RecommendGuid)
                .HasPrincipalKey(dish => dish.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.OnDeleteRestrict<DishMark, Mark>(x => x.Mark, x => x.DishMarks);
            modelBuilder.Entity<DishMark>().HasOne(x => x.Mark).WithMany(mark => mark.DishMarks)
                .HasForeignKey(dishMark => dishMark.MarkGuid)
                .HasPrincipalKey(mark => mark.RowGuid);
            modelBuilder.Entity<DishMark>().HasOne(x => x.Dish).WithMany(dish => dish.DishMarks)
                .HasForeignKey(dishMark => dishMark.DishGuid)
                .HasPrincipalKey(dish => dish.RowGuid);

            modelBuilder.Entity<DishModifier>().HasOne(x => x.Modifier).WithMany(modifier => modifier.DishModifiers)
                .HasForeignKey(dishModifier => dishModifier.ModifierGuid)
                .HasPrincipalKey(modifier => modifier.RowGuid)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DishModifier>().HasOne(x => x.Dish).WithMany(dish => dish.DishModifiers)
                .HasForeignKey(dishModifier => dishModifier.DishGuid)
                .HasPrincipalKey(dish => dish.RowGuid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MenuFolder>().HasOne(x => x.Parent).WithMany(menuFolder => menuFolder.MenuFolders)
                .HasForeignKey(menuFolder => menuFolder.ParentGuid)
                .HasPrincipalKey(parentFolder => parentFolder.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MenuFolderModifier>().HasOne(x => x.Modifier).WithMany(modifier => modifier.MenuFolderModifiers)
                .HasForeignKey(menuFolderModifier => menuFolderModifier.ModifierGuid)
                .HasPrincipalKey(modifier => modifier.RowGuid)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MenuFolderModifier>().HasOne(x => x.MenuFolder).WithMany(menuFolder => menuFolder.MenuFolderModifiers)
                .HasForeignKey(menuFolderModifier => menuFolderModifier.MenuFolderGuid)
                .HasPrincipalKey(menuFolder => menuFolder.RowGuid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.OnDeleteRestrict<Order, OrderStatus>(x => x.Status, x => x.Orders);
            modelBuilder.OnDeleteRestrict<Order, Source>(x => x.Source, x => x.Orders);
            modelBuilder.OnDeleteRestrict<Order, OrderType>(x => x.OrderType, x => x.Orders);
            modelBuilder.OnDeleteRestrict<Order, DeliveryType>(x => x.DeliveryType, x => x.Orders);
            modelBuilder.OnDeleteRestrict<Order, PaymentType>(x => x.PaymentType, x => x.Orders);
            modelBuilder.OnDeleteRestrict<Order, Restaurant>(x => x.Restaurant, x => x.Orders);
            modelBuilder.OnDeleteRestrict<Order, Customer>(x => x.Customer, x => x.Orders);
            modelBuilder.OnDeleteRestrict<Order, City>(x => x.City, x => x.Orders);
            modelBuilder.OnDeleteRestrict<Order, Street>(x => x.Street, x => x.Orders);

            modelBuilder.Entity<OrderDish>().HasOne(x => x.Order).WithMany(order => order.OrderDishes)
                .HasPrincipalKey(order => order.Id)
                .HasForeignKey(orderDish => orderDish.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDish>().HasIndex(x => x.OrderGuid);
            modelBuilder.Entity<OrderDish>().HasOne(x => x.Dish).WithMany(x => x.OrderDishes)
                .HasForeignKey(orderDish => orderDish.DishGuid)
                .HasPrincipalKey(dish => dish.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.OnDeleteRestrict<OrderDish, TaxType>(x => x.TaxType, x => x.OrderDishes);

            modelBuilder.Entity<OrderDishGarnish>().HasOne(x => x.Garnish).WithMany(x => x.OrderDishGarnishes)
                .HasForeignKey(dishGarnish => dishGarnish.GarnishGuid)
                .HasPrincipalKey(dishGarnish => dishGarnish.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderDishGarnish>().HasOne(x => x.Dish).WithMany(x => x.OrderDishGarnishes)
                .HasForeignKey(dishGarnish => dishGarnish.DishGuid)
                .HasPrincipalKey(dish => dish.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderDishGarnish>().HasOne(x => x.OrderDish).WithMany(x => x.OrderDishGarnishes)
                .HasForeignKey(dishGarnish => dishGarnish.OrderDishId)
                .HasPrincipalKey(orderDish => orderDish.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDishIngredient>().HasOne(x => x.Ingredient).WithMany(x => x.OrderDishIngredients)
                .HasForeignKey(dishIngredient => dishIngredient.IngredientGuid)
                .HasPrincipalKey(dishIngredient => dishIngredient.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderDishIngredient>().HasOne(x => x.Dish).WithMany(x => x.OrderDishIngredients)
                .HasForeignKey(dishIngredient => dishIngredient.DishGuid)
                .HasPrincipalKey(dish => dish.RowGuid)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderDishIngredient>().HasOne(x => x.OrderDish).WithMany(x => x.OrderDishIngredients)
                .HasForeignKey(dishIngredient => dishIngredient.OrderDishId)
                .HasPrincipalKey(orderDish => orderDish.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.OnDeleteRestrict<OrderStatusHistory, OrderStatus>(x => x.OrderStatusBeginning, x => x.OrderStatusBeginHistories);
            modelBuilder.OnDeleteRestrict<OrderStatusHistory, OrderStatus>(x => x.OrderStatusEnd, x => x.OrderStatusEndHistories);
            modelBuilder.OnDeleteRestrict<OrderStatusSequence, OrderStatus>(x => x.OrderStatusBeginning, x => x.OrderStatusBeginSequences);
            modelBuilder.OnDeleteRestrict<OrderStatusSequence, OrderStatus>(x => x.OrderStatusEnd, x => x.OrderStatusEndSequences);

            HasData(modelBuilder);
        }
        #endregion

        #region Private methods
        private async void HasData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbVersion>().HasData(new List<DbVersion>
            {
                new DbVersion { Id = 1, Version = "3.4.0" }
            });

            modelBuilder.Entity<CommunicationType>().HasData(new List<CommunicationType>
            {
                new CommunicationType { Id = 1, Name = "Мобильный" },
                new CommunicationType { Id = 2, Name = "Телефон" },
                new CommunicationType { Id = 3, Name = "Email" },
                new CommunicationType { Id = 4, Name = "Skype" }
            });

            modelBuilder.Entity<Source>().HasData(new List<Source>
            {
                new Source { Id = 1, Name = "Телефон" },
                new Source { Id = 2, Name = "Сайт" },
                new Source { Id = 3, Name = "Android" },
                new Source { Id = 4, Name = "IPhone" },
                new Source { Id = 5, Name = "Bonus" },
                new Source { Id = 6, Name = "Сайт моб.версия" },
                new Source { Id = 7, Name = "СушиЯ" },
                new Source { Id = 8, Name = "Экипаж" }
            });

            modelBuilder.Entity<OrderType>().HasData(new List<OrderType>
            {
                new OrderType { Id = 1, Name = "Доставка" },
                new OrderType { Id = 2, Name = "Самовынос" },
                new OrderType { Id = 3, Name = "Дозаказ доставка" },
                new OrderType { Id = 4, Name = "Дозаказ самовынос" },
                new OrderType { Id = 5, Name = "FoodTigra" }
            });

            modelBuilder.Entity<DeliveryType>().HasData(new List<DeliveryType>
            {
                new DeliveryType { Id = 1, Name = "На ближайшее время" },
                new DeliveryType { Id = 2, Name = "На конкретное время" },
                new DeliveryType { Id = 3, Name = "FoodTigra" }
            });

            modelBuilder.Entity<PaymentType>().HasData(new List<PaymentType>
            {
                new PaymentType { Id = 1, Name = "Касса" },
                new PaymentType { Id = 2, Name = "Кредитная карта" },
                new PaymentType { Id = 3, Name = "Оплата на сайте" },
                new PaymentType { Id = 4, Name = "Безнал" },
                new PaymentType { Id = 5, Name = "Сертификат" },
                new PaymentType { Id = 6, Name = "Взаимозачет" },
                new PaymentType { Id = 7, Name = "Неплательщики" },
            });

            modelBuilder.Entity<Restaurant>().HasData(new List<Restaurant>
            {
                new Restaurant { Id = 1, Name = "il Molino ул. Саксаганского 120", Manufacturer = "il Molino", Phone = "067 548 0857" },
                new Restaurant { Id = 2, Name = "il Molino проспект Победы, 45", Manufacturer = "il Molino", Phone = "(067) 659 7006" },
                new Restaurant { Id = 3, Name = "il Molino ул. Первомайская, 26", Manufacturer = "il Molino", Phone = "067 443 2411" }
            });

            modelBuilder.Entity<City>().HasData(new List<City>
            {
                new City { Id = 1, Name = "Киев" },
                new City { Id = 2, Name = "Вишневое"}
            });

            modelBuilder.Entity<StreetType>().HasData(new List<StreetType>
            {
                new StreetType { Id = 1, Name = "улица", NameShort = "ул." },
                new StreetType { Id = 2, Name = "переулок", NameShort = "пер." },
                new StreetType { Id = 3, Name = "бульвар", NameShort = "бул." },
                new StreetType { Id = 4, Name = "проспект", NameShort = "пр-т " }
            });

            modelBuilder.Entity<Street>().HasData(new List<Street>
            {
                new Street { Id = 1, Name = "Хрещатик", StreetTypeId = 1, CityId = 1 },
                new Street { Id = 2, Name = "Шевченко", StreetTypeId = 3, CityId = 1 },
                new Street { Id = 3, Name = "Петлюры", StreetTypeId = 2, CityId = 1 },
                new Street { Id = 4, Name = "Глубочицкая", StreetTypeId = 1, CityId = 1 },
                new Street { Id = 5, Name = "Владимирская", StreetTypeId = 1, CityId = 1 },
                new Street { Id = 6, Name = "Антоновича", StreetTypeId = 2, CityId = 1 },
                new Street { Id = 7, Name = "Саксаганского", StreetTypeId = 3, CityId = 1 },
                new Street { Id = 8, Name = "Терещенковская", StreetTypeId = 1, CityId = 1 },
                new Street { Id = 9, Name = "Софиевская", StreetTypeId = 1, CityId = 1 },

                new Street { Id = 10, Name = "Хрещатик 2", StreetTypeId = 1, CityId = 2 },
                new Street { Id = 11, Name = "Шевченко 2", StreetTypeId = 3, CityId = 2 },
                new Street { Id = 12, Name = "Петлюры 2", StreetTypeId = 2, CityId = 2 },
                new Street { Id = 13, Name = "Глубочицкая 2", StreetTypeId = 1, CityId = 2 },
                new Street { Id = 14, Name = "Владимирская 2", StreetTypeId = 1, CityId = 2 },
                new Street { Id = 15, Name = "Антоновича 2", StreetTypeId = 2, CityId = 2 },
                new Street { Id = 16, Name = "Саксаганского 2", StreetTypeId = 3, CityId = 2 },
                new Street { Id = 17, Name = "Терещенковская 2", StreetTypeId = 1, CityId = 2 },
                new Street { Id = 18, Name = "Софиевская 2", StreetTypeId = 1, CityId = 2 }
            });

            modelBuilder.Entity<OrderStatus>().HasData(new List<OrderStatus>
            {
                new OrderStatus { Id = 1, Name = "Новый с сайта", ColorStatus = "Gold" },
                new OrderStatus { Id = 2, Name = "Уточнение", ColorStatus = "BurlyWood" },
                new OrderStatus { Id = 3, Name = "Оформление", ColorStatus = "Plum" },
                new OrderStatus { Id = 4, Name = "Ожидание", ColorStatus = "MediumPurple" },
                new OrderStatus { Id = 5, Name = "Принят", ColorStatus = "Khaki" },
                new OrderStatus { Id = 6, Name = "Таймер", ColorStatus = "SteelBlue" },
                new OrderStatus { Id = 7, Name = "В работе", ColorStatus = "MediumSeaGreen" },
                new OrderStatus { Id = 8, Name = "Собран", ColorStatus = "YellowGreen" },
                new OrderStatus { Id = 9, Name = "Едет", ColorStatus = "Gray" },
                new OrderStatus { Id = 10, Name = "Доставлен", ColorStatus = "Transparent" },
                new OrderStatus { Id = 11, Name = "Закрыт", ColorStatus = "Transparent" },
                new OrderStatus { Id = 12, Name = "Отклонен", ColorStatus = "IndianRed" },
                new OrderStatus { Id = 13, Name = "Корзина", ColorStatus = "Transparent" }
            });

            modelBuilder.Entity<OrderStatusSequence>().HasData(new List<OrderStatusSequence>
            {
                // Новый с сайта
                new OrderStatusSequence { Id = 1, StatusIdBeginning = 1, StatusIdEnd = 2 },
                new OrderStatusSequence { Id = 2, StatusIdBeginning = 1, StatusIdEnd = 3 },
                new OrderStatusSequence { Id = 3, StatusIdBeginning = 1, StatusIdEnd = 12 },

                // Уточнение
                new OrderStatusSequence { Id = 4, StatusIdBeginning = 2, StatusIdEnd = 3 },
                new OrderStatusSequence { Id = 5, StatusIdBeginning = 2, StatusIdEnd = 12 },

                // Оформление
                new OrderStatusSequence { Id = 6, StatusIdBeginning = 3, StatusIdEnd = 2 },
                new OrderStatusSequence { Id = 7, StatusIdBeginning = 3, StatusIdEnd = 4 },
                new OrderStatusSequence { Id = 8, StatusIdBeginning = 3, StatusIdEnd = 5 },
                new OrderStatusSequence { Id = 9, StatusIdBeginning = 3, StatusIdEnd = 12 },

                // Ожидание
                new OrderStatusSequence { Id = 10, StatusIdBeginning = 4, StatusIdEnd = 5 },
                new OrderStatusSequence { Id = 11, StatusIdBeginning = 4, StatusIdEnd = 6 },
                new OrderStatusSequence { Id = 12, StatusIdBeginning = 4, StatusIdEnd = 12 },
                
                // Принят
                new OrderStatusSequence { Id = 13, StatusIdBeginning = 5, StatusIdEnd = 6 },
                new OrderStatusSequence { Id = 14, StatusIdBeginning = 5, StatusIdEnd = 7 },
                new OrderStatusSequence { Id = 15, StatusIdBeginning = 5, StatusIdEnd = 12 },

                // Таймер
                new OrderStatusSequence { Id = 16, StatusIdBeginning = 6, StatusIdEnd = 7 },
                new OrderStatusSequence { Id = 17, StatusIdBeginning = 6, StatusIdEnd = 12 },

                // В работе
                new OrderStatusSequence { Id = 18, StatusIdBeginning = 7, StatusIdEnd = 8 },
                new OrderStatusSequence { Id = 19, StatusIdBeginning = 7, StatusIdEnd = 12 },

                // Собран
                new OrderStatusSequence { Id = 20, StatusIdBeginning = 8, StatusIdEnd = 9 },
                new OrderStatusSequence { Id = 21, StatusIdBeginning = 8, StatusIdEnd = 12 },

                // Едет
                new OrderStatusSequence { Id = 22, StatusIdBeginning = 9, StatusIdEnd = 10 },
                new OrderStatusSequence { Id = 23, StatusIdBeginning = 9, StatusIdEnd = 12 },

                // Доставлен
                new OrderStatusSequence { Id = 24, StatusIdBeginning = 10, StatusIdEnd = 11 },
                new OrderStatusSequence { Id = 25, StatusIdBeginning = 10, StatusIdEnd = 12 },

                // Отклонен
                new OrderStatusSequence { Id = 26, StatusIdBeginning = 12, StatusIdEnd = 13 }
            });

            modelBuilder.Entity<TaxType>().HasData(new List<TaxType>
            {
                new TaxType { Id = 1, RowGuid = System.Guid.Parse("{C00E6FC6-31A4-43CA-8F0F-0D21B3E16D5B}"), Name = "Без НДС", Rate = 0 },
                new TaxType { Id = 2, RowGuid = System.Guid.Parse("{2C12AB99-06B0-444A-86F2-166BCC4928A4}"), Name = "НДС 20%", Rate = 20 },
                new TaxType { Id = 3, RowGuid = System.Guid.Parse("{DB50A83A-E422-45DF-BE12-8AC00DA3A171}"), Name = "НДС 7%", Rate = 7 }
            });

            var dishUnits = await ReadEntitiesFromJsonAsync<IList<DishUnit>>(nameof(DishUnits));
            modelBuilder.Entity<DishUnit>().HasData(dishUnits);

            var marks = await ReadEntitiesFromJsonAsync<IList<Mark>>(nameof(Marks));
            modelBuilder.Entity<Mark>().HasData(marks);

            var menuFolders = await ReadEntitiesFromJsonAsync<IList<MenuFolder>>(nameof(MenuFolders));
            modelBuilder.Entity<MenuFolder>().HasData(menuFolders);

            var dishes = await ReadEntitiesFromJsonAsync<List<Dish>>(nameof(Dishes));
            dishes.ForEach(dish => 
            {
                dish.Picture = null;
                dish.Thumbnail = null;
            });
            modelBuilder.Entity<Dish>().HasData(dishes);

            var garnishes = await ReadEntitiesFromJsonAsync<List<DishGarnish>>(nameof(DishGarnishes));
            garnishes.ForEach(garnish =>
            {
                garnish.Picture = null;
                garnish.Thumbnail = null;
            });
            modelBuilder.Entity<DishGarnish>().HasData(garnishes);

            var ingredients = await ReadEntitiesFromJsonAsync<List<DishIngredient>>(nameof(DishIngredients));
            ingredients.ForEach(ingredient =>
            {
                ingredient.Picture = null;
                ingredient.Thumbnail = null;
            });
            modelBuilder.Entity<DishIngredient>().HasData(ingredients);

            var dishMarks = await ReadEntitiesFromJsonAsync<IList<DishMark>>(nameof(DishMarks));
            modelBuilder.Entity<DishMark>().HasData(dishMarks);

            var recommends = await ReadEntitiesFromJsonAsync<IList<DishRecommend>>(nameof(DishRecommends));
            modelBuilder.Entity<DishRecommend>().HasData(recommends);

            modelBuilder.Entity<Variable>().HasData(new List<Variable>
            {
                new Variable { Id = 1, Name = "SignOfConsentDefault", Data = bool.TrueString, Description = "Признак согласия Гостя на использование его номера телефона для рассылки информационных сообщений" },
                new Variable { Id = 2, Name = "DeliveryTypeSoon_IntervalFrom", Data = "60", Description = "Настройка интервала \"от\" для доставки на ближайшее время (в минутах)" },
                new Variable { Id = 3, Name = "DeliveryTypeSoon_IntervalTo", Data = "180", Description = "Настройка интервала \"до\" для доставки на ближайшее время (в минутах)" },
                new Variable { Id = 4, Name = "ThumbnailWidth", Data = "68", Description = "Ширина превью картинки" },
                new Variable { Id = 5, Name = "ThumbnailHeight", Data = "56", Description = "Высота превью картинки" },
                new Variable { Id = 6, Name = "MinimumOrderSum", Data = "100", Description = "Минимальная сума заказа" }
            });
        }

        private async Task<T> ReadEntitiesFromJsonAsync<T>(string propertyName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = $"Inventory.Data.JsonFiles.{propertyName}.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var jsonString = await reader.ReadToEndAsync();
                    var entities = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
                    return entities;
                }
            }
        }

        #endregion
    }
}
