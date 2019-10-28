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
    public class Order : BaseEntity
    {
        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LastModifiedOn { get; set; }

        public Guid RowGuid { get; set; }

        [Required]
        [MaxLength(13)]
        public string PhoneNumber { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        public int? SourceId { get; set; }

        [ForeignKey(nameof(SourceId))]
        public virtual Source Source { get; set; }

        [Required]
        public int StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual OrderStatus Status { get; set; }

        public int OrderTypeId { get; set; }

        [ForeignKey(nameof(OrderTypeId))]
        public virtual OrderType OrderType { get; set; }

        /// <summary>
        /// Причина уточнения
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Заказ с подтверждением
        /// </summary>
        public bool IsConfirmation { get; set; }

        public DateTimeOffset? DeliveryDate { get; set; }

        public int DeliveryTypeId { get; set; }

        [ForeignKey(nameof(DeliveryTypeId))]
        public virtual DeliveryType DeliveryType { get; set; }

        public int RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }

        public int? CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }

        public int? StreetId { get; set; }

        [ForeignKey(nameof(StreetId))]
        public virtual Street Street { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// Номер квартиры/офиса
        /// </summary>
        public string Apartment { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public string Housing { get; set; }

        /// <summary>
        /// Подъезд
        /// </summary>
        public string Entrance { get; set; }

        /// <summary>
        /// Домофон
        /// </summary>
        public string Intercom { get; set; }

        /// <summary>
        /// Этаж
        /// </summary>
        public string Floor { get; set; }

        public int NumOfPeople { get; set; }

        public int PaymentTypeId { get; set; }

        [ForeignKey(nameof(PaymentTypeId))]
        public virtual PaymentType PaymentType { get; set; }

        /// <summary>
        /// Сдача с
        /// </summary>
        public decimal Change { get; set; }

        public bool CallOnArrival { get; set; }

        public string Comment { get; set; }

        public string CommentAddress { get; set; }

        public string CommentCustomer { get; set; }

        [Required]
        public string SearchTerms { get; set; }

        public virtual ICollection<OrderDish> OrderDishes { get; set; }
        public virtual ICollection<OrderStatusHistory> OrderStatusHistory { get; set; }
        public virtual ICollection<CourierOrder> CourierOrder { get; set; }

        public string BuildSearchTerms()
        {
            return $"{PhoneNumber} {City?.Name} {Street?.StreetType?.NameShort} {Street?.Name} {Restaurant?.Name}".Trim().ToLower();
        }
    }
}
