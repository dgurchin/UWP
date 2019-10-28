using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Inventory.BLL.DTO
{
    [DataContract]
    public class CustomerDTO
    {
        [DataMember, Required]
        public DateTimeOffset CreatedOn { get; set; }

        [DataMember, Required]
        public DateTimeOffset? LastModifiedOn { get; set; }

        [DataMember, Required]
        public Guid RowGuid { get; set; }

        [DataMember, Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [DataMember, MaxLength(50)]
        public string MiddleName { get; set; }

        [DataMember, MaxLength(50)]
        public string LastName { get; set; }

        [DataMember, MaxLength(50)]
        public string EmailAddress { get; set; }

        [DataMember]
        public DateTimeOffset? BirthDate { get; set; }

        [DataMember, DefaultValue(true)]
        public bool SignOfConsent { get; set; }

        [DataMember]
        public int? SourceId { get; set; }

        [DataMember]
        public bool IsBlockList { get; set; }

        [DataMember]
        public string SearchTerms { get; set; }

        [DataMember]
        public byte[] Picture { get; set; }

        [DataMember]
        public byte[] Thumbnail { get; set; }

        public string BuildSearchTerms() => $"{LastName} {FirstName} {EmailAddress}".ToLower();
    }
}
