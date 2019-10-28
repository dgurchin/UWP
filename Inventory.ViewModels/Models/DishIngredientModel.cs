using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class DishIngredientModel : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public Guid RowGuid { get; set; }

        public Guid DishGuid { get; set; }

        public int RowPosition { get; set; }

        public decimal Price { get; set; }

        public byte[] Picture { get; set; }
        public object PictureSource { get; set; }

        public byte[] Thumbnail { get; set; }
        public object ThumbnailSource { get; set; }
    }
}
