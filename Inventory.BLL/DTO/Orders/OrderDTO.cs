using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Inventory.BLL.DTO
{
    [DataContract]
    public class OrderDTO : BaseDTO
    {
        [DataMember, Required]
        public string PhoneNumber { get; set; }

        [DataMember, Required]
        public int OrderStateId { get; set; }

        [DataMember]
        public int? ContactId { get; set; }

        [DataMember]
        public int? CampaignId { get; set; }
    }
}
