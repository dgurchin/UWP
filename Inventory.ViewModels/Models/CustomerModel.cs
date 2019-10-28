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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Models
{
    public class CustomerModel : BaseModel
    {
        static public CustomerModel CreateEmpty() => new CustomerModel { Id = -1, IsEmpty = true };

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset? LastModifiedOn { get; set; }

        public Guid RowGuid { get; set; }

        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        [DefaultValue(true)]
        public bool SignOfConsent { get; set; }

        public int? SourceId { get; set; }

        public SourceModel Source { get; set; }

        public bool IsBlockList { get; set; }

        public byte[] Picture { get; set; }
        public object PictureSource { get; set; }

        public byte[] Thumbnail { get; set; }
        public object ThumbnailSource { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
        public string Initials => String.Format("{0}{1}", $"{FirstName} "[0], $"{LastName} "[0]).Trim().ToUpper();

        public override void Merge(ObservableObject source)
        {
            if (source is CustomerModel model)
            {
                Merge(model);
            }
        }

        public void Merge(CustomerModel source)
        {
            if (source != null)
            {
                Id = source.Id;
                CreatedOn = source.CreatedOn;
                LastModifiedOn = source.LastModifiedOn;
                RowGuid = source.RowGuid;
                FirstName = source.FirstName;
                MiddleName = source.MiddleName;
                LastName = source.LastName;
                BirthDate = source.BirthDate;
                SignOfConsent = source.SignOfConsent;
                SourceId = source.SourceId;
                Source = source.Source;
                IsBlockList = source.IsBlockList;
                Thumbnail = source.Thumbnail;
                ThumbnailSource = source.ThumbnailSource;
                Picture = source.Picture;
                PictureSource = source.PictureSource;
            }
        }

        public override string ToString()
        {
            return IsEmpty ? "Empty" : FullName;
        }
    }
}
