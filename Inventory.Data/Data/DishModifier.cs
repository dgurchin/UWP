using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class DishModifier : BaseEntity
    {
        public Guid RowGuid { get; set; }

        public Guid DishGuid { get; set; }

        [ForeignKey(nameof(DishGuid))]
        public virtual Dish Dish { get; set; }

        public Guid ModifierGuid { get; set; }

        [ForeignKey(nameof(ModifierGuid))]
        public virtual Modifier Modifier { get; set; }

        public bool IsRequired { get; set; }
    }
}
