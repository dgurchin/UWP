using System;

namespace Inventory.Models
{
    public class CommunicationModel : BaseModel
    {
        public Guid RowGuid { get; set; }

        public Guid CustomerGuid { get; set; }

        public string Name { get; set; }

        public bool IsPrimary { get; set; }

        public int TypeId { get; set; }

        public int CustomerId { get; set; }

        public bool IsIgnore { get; set; }

        public override void Merge(ObservableObject source)
        {
            if (source is CommunicationModel)
            {
                Merge((CommunicationModel)source);
            }
        }
        
        public void Merge(CommunicationModel source)
        {
            if (source == null)
            {
                return;
            }

            Id = source.Id;
            RowGuid = source.RowGuid;
            CustomerGuid = source.CustomerGuid;
            Name = source.Name;
            IsPrimary = source.IsPrimary;
            CustomerId = source.CustomerId;
            IsIgnore = source.IsIgnore;
        }
    }
}
