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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Data
{
    public class Customer : BaseEntity
    {
        [Required]
        public DateTimeOffset CreatedOn { get; set; }

        [Required]
        public DateTimeOffset? LastModifiedOn { get; set; }

        public Guid RowGuid { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string MiddleName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string EmailAddress { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        /// <summary>
        /// Признак согласия
        /// </summary>
        [DefaultValue(true)]
        public bool SignOfConsent { get; set; }

        public int? SourceId { get; set; }

        [ForeignKey(nameof(SourceId))]
        public virtual Source Source { get; set; }

        public bool IsBlockList { get; set; }

        public string SearchTerms { get; set; }

        public byte[] Picture { get; set; }

        public byte[] Thumbnail { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string BuildSearchTerms() => $"{LastName} {FirstName} {EmailAddress}".ToLower();
    }
}
