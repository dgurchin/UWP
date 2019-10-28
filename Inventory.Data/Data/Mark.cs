using System;
using System.Collections.Generic;

namespace Inventory.Data
{
    /// <summary>
    /// Особеность (изображение: острое и т.д.) 
    /// </summary>
    public class Mark : BaseEntity
    {
        public Guid RowGuid { get; set; }

        public byte[] Picture { get; set; }

        public virtual ICollection<DishMark> DishMarks { get; set; }
    }
}
