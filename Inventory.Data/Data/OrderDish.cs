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
    /// Блюдо в заказе
    /// </summary>
    public class OrderDish : BaseEntity
    {
        public Guid OrderGuid { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Required]
        public Guid DishGuid { get; set; }

        [ForeignKey(nameof(DishGuid))]
        public virtual Dish Dish { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int TaxTypeId { get; set; }

        [ForeignKey(nameof(TaxTypeId))]
        public virtual TaxType TaxType { get; set; }

        public virtual ICollection<OrderDishGarnish> OrderDishGarnishes { get; set; }
        public virtual ICollection<OrderDishIngredient> OrderDishIngredients { get; set; }
    }
}
