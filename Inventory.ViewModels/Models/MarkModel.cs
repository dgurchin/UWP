using System;

namespace Inventory.Models
{
    public class MarkModel : BaseModel
    {
        public Guid RowGuid { get; set; }

        public byte[] Picture { get; set; }
        public object PictureSource { get; set; }
    }
}
