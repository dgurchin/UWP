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
using System.ComponentModel.DataAnnotations;

using Inventory.Models.Enums;

namespace Inventory.Models
{
    public class OrderModel : BaseModel
    {
        public static OrderModel CreateEmpty() => new OrderModel { Id = -1, CustomerId = -1, IsEmpty = true };

        #region Private fields
        private Guid _rowGuid;
        private string _phoneNumber;
        private int _customerId;
        private CustomerModel _customer;
        private string _editableCustomerName;
        private int _statusId;
        private int _orderTypeId;
        private DateTimeOffset? _deliveryDate;
        private TimeSpan _deliveryTime;
        private bool _isDeliveryDateReadOnly;
        private int _deliveryTypeId;
        private int _restaurantId;
        private RestaurantModel _restaurant;
        private int _paymentTypeId;
        private string _reason;
        private bool _isConfirmation;
        private decimal _minOrderSum;
        private int? _sourceId;
        private int _numOfPeople;
        private decimal _change;
        private int _cityId;
        private CityModel _city;
        private int _streetId;
        private StreetModel _streetModel;
        private string _house;
        private string _appartment;
        private string _housing;
        private string _entrance;
        private string _intercom;
        private string _floor;
        private string _comment;
        private string _commentAddress;
        private string _commentCustomer;
        private bool _callOnArrival;
        #endregion

        #region Properties

        public DateTimeOffset CreatedOn { get; set; }

        /// <summary>
        /// Время заказа - будет записано в <see cref="CreatedOn"/> при сохранении/обновлении
        /// </summary>
        public TimeSpan CreatedOnTime { get; set; }

        public Guid RowGuid
        {
            get => _rowGuid;
            set => Set(ref _rowGuid, value);
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => Set(ref _phoneNumber, value);
        }

        public int CustomerId
        {
            get => _customerId;
            set
            {
                if (Set(ref _customerId, value))
                {
                    NotifyPropertyChanged(nameof(IsNewCustomer));
                    NotifyPropertyChanged(nameof(EditableCustomerName));
                }
            }
        }

        public CustomerModel Customer
        {
            get => _customer;
            set
            {
                if (Set(ref _customer, value))
                {
                    NotifyPropertyChanged(nameof(EditableCustomerName));
                }
            }
        }

        public string EditableCustomerName
        {
            get
            {
                if (Customer == null)
                {
                    return _editableCustomerName?.Trim();
                }

                return Customer.FullName?.Trim();
            }
            set
            {
                if (Customer != null)
                {
                    _editableCustomerName = null;
                }
                else
                {
                    Set(ref _editableCustomerName, value);
                }
                NotifyPropertyChanged(nameof(IsNewCustomer));
            }
        }

        public bool IsNewCustomer
        {
            get
            {
                return CustomerId <= 0 && !string.IsNullOrEmpty(EditableCustomerName);
            }
        }

        public int? SourceId
        {
            get => _sourceId;
            set => Set(ref _sourceId, value);
        }

        public SourceModel OrderSource { get; set; }

        public int StatusId
        {
            get => _statusId;
            set
            {
                if (Set(ref _statusId, value))
                {
                    NotifyPropertyChanged(nameof(IsReasonVisible));
                    NotifyPropertyChanged(nameof(CanEditRestaurant));
                }
            }
        }

        public OrderStatusModel Status { get; set; }

        /// <summary>
        /// Причина уточнения
        /// </summary>
        public string Reason
        {
            get => _reason;
            set => Set(ref _reason, value);
        }

        public bool IsReasonVisible => (OrderStatusEnum)StatusId == OrderStatusEnum.Clarification;

        /// <summary>
        /// Заказ с подтверждением
        /// </summary>
        public bool IsConfirmation
        {
            get => _isConfirmation;
            set => Set(ref _isConfirmation, value);
        }

        public int OrderTypeId
        {
            get => _orderTypeId;
            set
            {
                if (Set(ref _orderTypeId, value))
                {
                    NotifyPropertyChanged(nameof(IsAddressRequired));
                }
            }
        }

        public OrderTypeModel OrderType { get; set; }

        public DateTimeOffset? DeliveryDate
        {
            get => _deliveryDate;
            set
            {
                if (Set(ref _deliveryDate, value))
                {
                    NotifyPropertyChanged(nameof(DeliveryDateTime));
                }
            }
        }

        /// <summary>
        /// Время доставки - будет записано в <see cref="DeliveryDate"/> при сохранении/обновлении
        /// </summary>
        public TimeSpan DeliveryTime
        {
            get => _deliveryTime;
            set
            {
                if (Set(ref _deliveryTime, value))
                {
                    NotifyPropertyChanged(nameof(DeliveryDateTime));
                }
            }
        }

        public DateTimeOffset? DeliveryDateTime => CombineDateTimeOffsetWithTime(DeliveryDate, DeliveryTime);

        public bool IsDeliveryDateReadOnly
        {
            get => _isDeliveryDateReadOnly;
            set => Set(ref _isDeliveryDateReadOnly, value);
        }

        public int DeliveryTypeId
        {
            get => _deliveryTypeId;
            set => Set(ref _deliveryTypeId, value);
        }

        public DeliveryTypeModel DeliveryType { get; set; }

        public int RestaurantId
        {
            get => _restaurantId;
            set => Set(ref _restaurantId, value);
        }

        public RestaurantModel Restaurant
        {
            get => _restaurant;
            set => Set(ref _restaurant, value);
        }

        /// <summary>
        /// Ресторан можно менять если статус заказа еще не в работе
        /// </summary>
        public bool CanEditRestaurant => StatusId < (int)OrderStatusEnum.InProcess || (IsNew && StatusId >= (int)OrderStatusEnum.Canceled);

        public int NumOfPeople
        {
            get => _numOfPeople;
            set => Set(ref _numOfPeople, value);
        }

        public int PaymentTypeId
        {
            get => _paymentTypeId;
            set
            {
                Set(ref _paymentTypeId, value);
                NotifyPropertyChanged(nameof(IsChangeVisible));
            }
        }
        public PaymentTypeModel PaymentType { get; set; }

        /// <summary>
        /// Сдача с
        /// </summary>
        public decimal Change
        {
            get => _change;
            set => Set(ref _change, value);
        }

        public bool IsChangeVisible => (PaymentTypeEnum)PaymentTypeId == PaymentTypeEnum.Cash;

        public string Comment
        {
            get => _comment;
            set => Set(ref _comment, value);
        }

        public string CommentAddress
        {
            get => _commentAddress;
            set => Set(ref _commentAddress, value);
        }

        public string CommentCustomer
        {
            get => _commentCustomer;
            set => Set(ref _commentCustomer, value);
        }

        /// <summary>
        /// Признак набрать по прибытии
        /// </summary>
        public bool CallOnArrival
        {
            get => _callOnArrival;
            set => Set(ref _callOnArrival, value);
        }

        #endregion

        #region Address

        public bool IsAddressRequired
        {
            get => OrderTypeId != (int)OrderTypeEnum.Takeaway && OrderTypeId != (int)OrderTypeEnum.OrderTakeaway;
        }

        public bool IsAddressEmpty
        {
            get
            {
                return CityId <= 0 && StreetId <= 0 && string.IsNullOrEmpty(House) && string.IsNullOrEmpty(Apartment);
            }
        }

        public int CityId
        {
            get => _cityId;
            set
            {
                if (Set(ref _cityId, value))
                {
                    NotifyPropertyChanged(nameof(IsAddressEmpty));
                }
            }
        }

        public CityModel City
        {
            get => _city;
            set => Set(ref _city, value);
        }

        public int StreetId
        {
            get => _streetId;
            set
            {
                if (Set(ref _streetId, value))
                {
                    NotifyPropertyChanged(nameof(IsAddressEmpty));
                }
            }
        }

        public StreetModel Street
        {
            get => _streetModel;
            set => Set(ref _streetModel, value);
        }

        [Required]
        /// <summary>
        /// Номер дома
        /// </summary>
        public string House
        {
            get => _house;
            set
            {
                if (Set(ref _house, value))
                {
                    NotifyPropertyChanged(nameof(IsAddressEmpty));
                }
            }
        }

        /// <summary>
        /// Номер квартиры/офиса
        /// </summary>
        public string Apartment
        {
            get => _appartment;
            set
            {
                if (Set(ref _appartment, value))
                {
                    NotifyPropertyChanged(nameof(IsAddressEmpty));
                }
            }
        }

        /// <summary>
        /// Корпус
        /// </summary>
        public string Housing
        {
            get => _housing;
            set => Set(ref _housing, value);
        }

        /// <summary>
        /// Подъезд
        /// </summary>
        public string Entrance
        {
            get => _entrance;
            set => Set(ref _entrance, value);
        }

        /// <summary>
        /// Домофон
        /// </summary>
        public string Intercom
        {
            get => _intercom;
            set => Set(ref _intercom, value);
        }

        /// <summary>
        /// Этаж
        /// </summary>
        public string Floor
        {
            get => _floor;
            set => Set(ref _floor, value);
        }

        #endregion

        /// <summary>
        /// Минимальная сума заказа
        /// </summary>
        public decimal MinOrderSum
        {
            get => _minOrderSum;
            set
            {
                if (Set(ref _minOrderSum, value))
                {
                    NotifyPropertyChanged(nameof(IsMinOrderSumVisible));
                }
            }
        }

        /// <summary>
        /// Отображать ли <see cref="MinOrderSum"/>
        /// </summary>
        public bool IsMinOrderSumVisible => MinOrderSum > 0;

        public override void Merge(ObservableObject source)
        {
            if (source is OrderModel model)
            {
                Merge(model);
            }
        }

        public void Merge(OrderModel source)
        {
            if (source != null)
            {
                Id = source.Id;
                CreatedOn = source.CreatedOn;
                CreatedOnTime = source.CreatedOnTime;
                RowGuid = source.RowGuid;

                PhoneNumber = source.PhoneNumber;
                CustomerId = source.CustomerId;
                Customer = source.Customer;
                EditableCustomerName = source.EditableCustomerName;

                StatusId = source.StatusId;
                Status = source.Status;
                SourceId = source.SourceId;
                OrderSource = source.OrderSource;
                OrderTypeId = source.OrderTypeId;
                OrderType = source.OrderType;
                Reason = source.Reason;
                IsConfirmation = source.IsConfirmation;

                DeliveryDate = source.DeliveryDate;
                DeliveryTime = source.DeliveryTime;
                IsDeliveryDateReadOnly = source.IsDeliveryDateReadOnly;
                DeliveryTypeId = source.DeliveryTypeId;
                DeliveryType = source.DeliveryType;
                RestaurantId = source.RestaurantId;
                Restaurant = source.Restaurant;
                CallOnArrival = source.CallOnArrival;

                CityId = source.CityId;
                City = source.City;
                StreetId = source.StreetId;
                Street = source.Street;
                Apartment = source.Apartment;
                House = source.House;
                Housing = source.Housing;
                Entrance = source.Entrance;
                Intercom = source.Intercom;
                Floor = source.Floor;

                PaymentTypeId = source.PaymentTypeId;
                PaymentType = source.PaymentType;
                Change = source.Change;
                NumOfPeople = source.NumOfPeople;

                Comment = source.Comment;
                CommentAddress = source.CommentAddress;
                CommentCustomer = source.CommentCustomer;
            }
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        private DateTimeOffset? CombineDateTimeOffsetWithTime(DateTimeOffset? dto, TimeSpan time)
        {
            DateTimeOffset? dateTime = dto;
            if (dateTime != null)
            {
                var date = dto.Value;
                dateTime = new DateTimeOffset(date.Year, date.Month, date.Day,
                    time.Hours, time.Minutes, time.Seconds, date.Offset);
            }
            return dateTime;
        }
    }
}

