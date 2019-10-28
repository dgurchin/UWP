namespace Inventory.Models
{
    public class OrderStatusSequenceModel : BaseModel
    {
        public int? StatusIdBeginning { get; set; }
        public int? StatusIdEnd { get; set; }
        public string Direction { get; set; }
        public string Algorithm { get; set; }
        public string Comment { get; set; }
    }
}
