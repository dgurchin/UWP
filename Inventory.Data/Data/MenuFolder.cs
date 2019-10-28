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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    /// <summary>
    /// Папка к которой относятся блюда
    /// </summary>
    public class MenuFolder : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        public Guid RowGuid { get; set; }

        public int RowNumber { get; set; }

        public Guid? ParentGuid { get; set; }

        [ForeignKey(nameof(ParentGuid))]
        public virtual MenuFolder Parent { get; set; }

        public byte[] Picture { get; set; }
        public byte[] Thumbnail { get; set; }

        public virtual ICollection<Dish> Dishes { get; set; }

        public virtual ICollection<MenuFolder> MenuFolders { get; set; }

        public virtual ICollection<MenuFolderModifier> MenuFolderModifiers { get; set; }
    }
}

