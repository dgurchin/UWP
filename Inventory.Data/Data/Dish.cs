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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    /// <summary>
    /// Блюдо
    /// </summary>
    public class Dish : BaseEntity
    {
        public Guid RowGuid { get; set; }

        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        [Required]
        public DateTimeOffset LastModifiedOn { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid MenuFolderGuid { get; set; }

        [ForeignKey(nameof(MenuFolderGuid))]
        public virtual MenuFolder MenuFolder { get; set; }

        /// <summary>
        /// Штрих-код
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Энергетическая ценность продукта
        /// </summary>
        public string EnergyValue { get; set; }

        /// <summary>
        /// Guid единицы измерения блюда
        /// </summary>
        public Guid DishUnitGuid { get; set; }

        public virtual DishUnit DishUnit { get; set; }

        /// <summary>
        /// Количество <see cref="DishUnit"/>
        /// </summary>
        public decimal UnitCount { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Guid TaxTypeGuid { get; set; }

        public virtual TaxType TaxType { get; set; }

        public bool IsExcise { get; set; }

        public string SearchTerms { get; set; }

        public byte[] Picture { get; set; }
        public byte[] Thumbnail { get; set; }

        public virtual ICollection<OrderDish> OrderDishes { get; set; }

        public virtual ICollection<DishMark> DishMarks { get; set; }

        public virtual ICollection<DishModifier> DishModifiers { get; set; }

        public virtual ICollection<DishGarnish> DishGarnishes { get; set; }

        public virtual ICollection<DishIngredient> DishIngredients { get; set; }

        public virtual ICollection<DishRecommend> DishRecommends { get; set; }

        public virtual ICollection<DishRecommend> RecommendDishes { get; set; }

        public virtual ICollection<OrderDishGarnish> OrderDishGarnishes { get; set; }

        public virtual ICollection<OrderDishIngredient> OrderDishIngredients { get; set; }

        public virtual ICollection<StopSheet> StopSheets { get; set; }

        public string BuildSearchTerms() => $"{Name}".ToLower();
    }
}
