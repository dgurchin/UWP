using System.Runtime.Serialization;

namespace Inventory.BLL.DTO
{
    [DataContract]
    public class BaseDTO
    {
        [DataMember]
        public int Id { get; set; }
    }
}
