using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class MenuFolderModifier : BaseEntity
    {
        public Guid RowGuid { get; set; }

        public Guid MenuFolderGuid { get; set; }

        [ForeignKey(nameof(MenuFolderGuid))]
        public virtual MenuFolder MenuFolder { get; set; }

        public Guid ModifierGuid { get; set; }

        [ForeignKey(nameof(ModifierGuid))]
        public virtual Modifier Modifier { get; set; }
    }
}
