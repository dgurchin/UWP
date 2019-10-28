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

using Inventory.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Inventory.Models
{
    public class OrderDishModel : BaseModel
    {
        private decimal _quantity;
        private int _taxTypeId;
        private decimal _discount;
        private decimal _price;
        private OrderGarnishModel _orderGarnish;
        private IList<OrderGarnishModel> _garnishes;
        private IList<OrderIngredientModel> _ingredients;

        public Guid OrderGuid { get; set; }

        public int? OrderId { get; set; }

        public Guid DishGuid { get; set; }

        public DishModel Dish { get; set; }

        [Required]
        public decimal Quantity
        {
            get => _quantity;
            set { if (Set(ref _quantity, value)) UpdateTotals(); }
        }

        [Required]
        public decimal Price
        {
            get => _price;
            set
            {
                if (Set(ref _price, value))
                {
                    UpdateTotals();
                }
            }
        }

        [Required]
        public int TaxTypeId
        {
            get => _taxTypeId;
            set { if (Set(ref _taxTypeId, value)) UpdateTotals(); }
        }

        public decimal Discount
        {
            get => _discount;
            set { if (Set(ref _discount, value)) UpdateTotals(); }
        }

        public decimal Subtotal
        {
            get
            {
                if (Ingredients?.Count > 0)
                {
                    decimal ingredientSum = Ingredients.Where(x => x.IsSelected).Select(x => x.Ingredient.Price).Sum();
                    return Quantity * Price + ingredientSum;
                }
                return Quantity * Price;
            }
        }

        public decimal Total => (Subtotal - Discount) * (1 + LookupTablesProxy.Instance.GetTaxRate(TaxTypeId) / 100m);

        public OrderGarnishModel SelectedGarnish
        {
            get => _orderGarnish;
            set
            {
                if (Set(ref _orderGarnish, value))
                {
                    UpdateGarnishPrice();
                }
            }
        }

        public IList<OrderGarnishModel> Garnishes
        {
            get => _garnishes;
            set
            {
                if (Set(ref _garnishes, value))
                {
                    NotifyPropertyChanged(nameof(HasGarnishes));
                }
            }
        }

        public bool HasGarnishes => Garnishes?.Count > 0;

        public IList<OrderIngredientModel> Ingredients
        {
            get => _ingredients;
            set
            {
                if (Set(ref _ingredients, value))
                {
                    NotifyPropertyChanged(nameof(HasIngredients));
                    UpdateTotals();
                }
            }
        }

        public bool HasIngredients => Ingredients?.Count > 0;

        public void UpdateTotals()
        {
            NotifyPropertyChanged(nameof(Subtotal));
            NotifyPropertyChanged(nameof(Total));
        }

        private void UpdateGarnishPrice()
        {
            Price = SelectedGarnish?.Price ?? 0m;
        }

        public override void Merge(ObservableObject source)
        {
            if (source is OrderDishModel model)
            {
                Merge(model);
            }
        }

        public void Merge(OrderDishModel source)
        {
            if (source != null)
            {
                Id = source.Id;
                OrderId = source.OrderId;
                OrderGuid = source.OrderGuid;
                DishGuid = source.DishGuid;
                Dish = source.Dish;
                Quantity = source.Quantity;
                Price = source.Price;
                Discount = source.Discount;
                TaxTypeId = source.TaxTypeId;
            }
        }
    }
}
