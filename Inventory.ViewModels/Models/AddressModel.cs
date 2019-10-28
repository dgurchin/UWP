using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class AddressModel : BaseModel
    {
        public Guid RowGuid { get; set; }

        public Guid CustomerGuid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CustomerId { get; set; }

        private int _cityId;
        [Required]
        public int CityId
        {
            get => _cityId;
            set => Set(ref _cityId, value);
        }

        private CityModel _city;
        public CityModel City
        {
            get => _city;
            set => Set(ref _city, value);
        }

        private int _streetId;
        [Required]
        public int StreetId
        {
            get => _streetId;
            set => Set(ref _streetId, value);
        }

        private StreetModel _street;
        public StreetModel Street
        {
            get => _street;
            set => Set(ref _street, value);
        }

        [Required]
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

        public bool IsPrimary { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is AddressModel model)
            {
                Merge(model);
            }
        }

        public void Merge(AddressModel source)
        {
            if (source == null)
            {
                return;
            }

            Id = source.Id;
            RowGuid = source.RowGuid;
            CustomerGuid = source.CustomerGuid;
            Name = source.Name;
            CustomerId = source.CustomerId;
            CityId = source.CityId;
            StreetId = source.StreetId;
            House = source.House;
            Apartment = source.Apartment;
            Housing = source.Housing;
            Entrance = source.Entrance;
            Intercom = source.Intercom;
            Floor = source.Floor;
            IsPrimary = source.IsPrimary;
        }

        public override string ToString()
        {
            return $"{Name}".Trim();
        }
    }
}
