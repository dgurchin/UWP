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
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Inventory.Data.Services
{
    public interface IDataSource : IDisposable
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<City> Cities { get; set; }
        DbSet<Street> Streets { get; set; }
        DbSet<StreetType> StreetTypes { get; set; }

        DbSet<Communication> Communications { get; set; }
        DbSet<CommunicationType> CommunicationTypes { get; set; }

        DbSet<Customer> Customers { get; set; }

        DbSet<DbVersion> DbVersion { get; set; }
        DbSet<DeliveryType> DeliveryTypes { get; set; }

        DbSet<Dish> Dishes { get; set; }
        DbSet<DishGarnish> DishGarnishes { get; set; }
        DbSet<DishIngredient> DishIngredients { get; set; }
        DbSet<DishMark> DishMarks { get; set; }
        DbSet<DishModifier> DishModifiers { get; set; }
        DbSet<DishRecommend> DishRecommends { get; set; }
        DbSet<DishUnit> DishUnits { get; set; }

        DbSet<Mark> Marks { get; set; }
        DbSet<Modifier> Modifiers { get; set; }

        DbSet<MenuFolder> MenuFolders { get; set; }
        DbSet<MenuFolderModifier> MenuFolderModifiers { get; set; }

        DbSet<Order> Orders { get; set; }
        DbSet<OrderDish> OrderDishes { get; set; }
        DbSet<OrderDishGarnish> OrderDishGarnishes { get; set; }
        DbSet<OrderDishIngredient> OrderDishIngredients { get; set; }
        DbSet<OrderStatus> OrderStatuses { get; set; }
        DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        DbSet<OrderStatusSequence> OrderStatusSequences { get; set; }
        DbSet<OrderType> OrderTypes { get; set; }

        DbSet<PaymentType> PaymentTypes { get; set; }
        DbSet<Restaurant> Restaurants { get; set; }
        DbSet<Source> Sources { get; set; }
        DbSet<TaxType> TaxTypes { get; set; }
        DbSet<Variable> Variables { get; set; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
