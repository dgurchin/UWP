using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public class Modifier : BaseEntity
    {
        public Guid RowGuid { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsRequired { get; set; }

        public virtual ICollection<DishModifier> DishModifiers { get; set; }

        public virtual ICollection<MenuFolderModifier> MenuFolderModifiers { get; set; }
    }
}
