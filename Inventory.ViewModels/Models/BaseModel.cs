namespace Inventory.Models
{
    public class BaseModel : ObservableObject
    {
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (Set(ref _id, value))
                {
                    NotifyPropertyChanged(nameof(IsNew));
                }
            }
        }

        public bool IsNew => Id <= 0;
    }
}
