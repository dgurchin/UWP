using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventory.Data.Services
{
    internal static class ModelBuilderExtensions
    {
        public static void RemovePluralizingTableNameConvention(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
        }

        public static void OnDeleteRestrict<TEntity, TRelatedEntity>(this ModelBuilder modelBuilder,
            Expression<Func<TEntity, TRelatedEntity>> navigationProperty,
            Expression<Func<TRelatedEntity, IEnumerable<TEntity>>> navigationCollection)
            where TEntity : class
            where TRelatedEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .HasOne(navigationProperty)
                .WithMany(navigationCollection)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public static void OnDeleteCascade<TEntity, TRelatedEntity>(this ModelBuilder modelBuilder,
            Expression<Func<TEntity, TRelatedEntity>> navigationProperty,
            Expression<Func<TRelatedEntity, IEnumerable<TEntity>>> navigationCollection)
            where TEntity : class
            where TRelatedEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .HasOne(navigationProperty)
                .WithMany(navigationCollection)
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <summary>
        ///   Based on BackDoorManUC's suggestion: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754.
        /// </summary>
        public static ModelBuilder UseValueConverterForType<T>(this ModelBuilder modelBuilder, ValueConverter converter, string sqlType = null, bool useTypeNameSuffix = false)
        {
            return modelBuilder.UseValueConverterForType(typeof(T), converter, sqlType, useTypeNameSuffix);
        }

        public static ModelBuilder UseValueConverterForType(this ModelBuilder modelBuilder, Type type, ValueConverter converter, string sqlType = null, bool useTypeNameSuffix = false)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // note that entityType.GetProperties() will throw an exception, so we have to use reflection 
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == type);
                foreach (var property in properties)
                {
                    var prop = modelBuilder.Entity(entityType.Name).Property(property.Name);
                    prop.HasConversion(converter);
                    if (sqlType != null)
                        prop.HasColumnType(sqlType);
                    if (useTypeNameSuffix)
                        // Apparantly property.Name doesn't provide the current property name
                        prop.HasColumnName($"{prop.Metadata.Relational().ColumnName}_{type.Name}");
                }
            }

            return modelBuilder;
        }
    }
}
