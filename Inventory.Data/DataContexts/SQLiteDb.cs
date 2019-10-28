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

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    public class SQLiteDb : DbContext, IDataSource
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

        public SQLiteDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Protected methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<OrderItem>().HasKey(e => new { e.OrderId, e.OrderLine });
            //modelBuilder.Entity<OrderStatusHistory>().HasKey(e => new { e.OrderId, e.OrderLine });
        }

        #endregion
    }
}
