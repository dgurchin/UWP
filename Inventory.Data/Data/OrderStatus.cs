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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class OrderStatus : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ColorStatus { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        [NotMapped]
        public virtual ICollection<OrderStatusHistory> OrderStatusBeginHistories { get; set; }
        [NotMapped]
        public virtual ICollection<OrderStatusHistory> OrderStatusEndHistories { get; set; }

        [NotMapped]
        public virtual ICollection<OrderStatusSequence> OrderStatusBeginSequences { get; set; }
        [NotMapped]
        public virtual ICollection<OrderStatusSequence> OrderStatusEndSequences { get; set; }
    }
}
