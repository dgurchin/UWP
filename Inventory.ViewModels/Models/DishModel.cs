#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class DishModel : BaseModel
    {
        static public DishModel CreateEmpty() => new DishModel { IsEmpty = true };

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset LastModifiedOn { get; set; }

        public Guid RowGuid { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid MenuFolderGuid { get; set; }

        public MenuFolderModel MenuFolder { get; set; }

        /// <summary>
        /// Штрих-код
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Энергетическая ценность продукта
        /// </summary>
        public string EnergyValue { get; set; }

        public Guid DishUnitGuid { get; set; }

        public DishUnitModel DishUnit { get; set; }

        public decimal UnitCount { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Guid TaxTypeGuid { get; set; }

        public TaxTypeModel TaxType { get; set; }

        public string SearchTerms { get; set; }

        public byte[] Picture { get; set; }
        public object PictureSource { get; set; }

        public byte[] Thumbnail { get; set; }
        public object ThumbnailSource { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is DishModel model)
            {
                Merge(model);
            }
        }

        public void Merge(DishModel source)
        {
            if (source != null)
            {
                Id = source.Id;
                CreatedOn = source.CreatedOn;
                LastModifiedOn = source.LastModifiedOn;
                Name = source.Name;
                Description = source.Description;
                MenuFolderGuid = source.MenuFolderGuid;
                Price = source.Price;
                TaxTypeGuid = source.TaxTypeGuid;
                Barcode = source.Barcode;
                EnergyValue = source.EnergyValue;
                DishUnitGuid = source.DishUnitGuid;
                UnitCount = source.UnitCount;
                RowGuid = source.RowGuid;
                Picture = source.Picture;
                PictureSource = source.PictureSource;
                Thumbnail = source.Thumbnail;
                ThumbnailSource = source.ThumbnailSource;

                if (MenuFolderGuid != Guid.Empty && source.MenuFolder != null)
                {
                    MenuFolder = new MenuFolderModel
                    {
                        Id = source.MenuFolder.Id,
                        RowGuid = source.MenuFolder.RowGuid,
                        ParentGuid = source.MenuFolder.ParentGuid,
                        SequenceNumber = source.MenuFolder.SequenceNumber,
                        Name = source.MenuFolder.Name,
                        Description = source.MenuFolder.Description
                    };
                }

                if (TaxTypeGuid != Guid.Empty && source.TaxType != null)
                {
                    TaxType = new TaxTypeModel
                    {
                        Id = source.TaxType.Id,
                        RowGuid = source.TaxType.RowGuid,
                        Name = source.TaxType.Name,
                        Rate = source.TaxType.Rate
                    };
                }

                if (DishUnitGuid != Guid.Empty && source.DishUnit != null)
                {
                    DishUnit = new DishUnitModel { Id = source.DishUnit.Id, Name = source.DishUnit.Name };
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
