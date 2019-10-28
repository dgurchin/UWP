namespace Inventory.Models
{
    public class StreetModel : BaseModel
    {
        public string Name { get; set; }

        public int StreetTypeId { get; set; }

        public StreetTypeModel StreetTypeModel { get; set; }

        public int CityId { get; set; }

        public string DisplayName
        {
            get => $"{StreetTypeModel?.NameShort} {Name}".Trim();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
