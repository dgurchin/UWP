using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Inventory.Data
{
    /// <summary>
    /// Адрес клиента
    /// </summary>
    public class Address : BaseEntity
    {
        public Guid RowGuid { get; set; }

        public Guid CustomerGuid { get; set; }

        /// <summary>
        /// Название улицы/проспекта/площади
        /// </summary>
        [Required]
        public string Name { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        [Required]
        public int CityId { get; set; }

        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        [Required]
        public int StreetId { get; set; }

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

        public bool IsPrimary { get; set; }

        public void BuildName()
        {
            Name = $"{City?.Name}, {Street?.StreetType?.NameShort} {Street?.Name}, дом №{House} кв.{Apartment}/{Housing}, домофон {Intercom}".Trim();
        }
    }
}

