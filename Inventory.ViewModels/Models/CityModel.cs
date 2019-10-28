namespace Inventory.Models
{
    public class CityModel : BaseModel
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
