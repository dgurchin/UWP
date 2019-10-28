
using System;

using Inventory.Services;

namespace Inventory.Models
{
    public class OrderStatusHistoryModel : BaseModel
    {
        public int OrderId { get; set; }
        public int OrderLine { get; set; }

        /// <summary>
        /// Статус перед переходом
        /// </summary>
        public int StatusIdBeginning { get; set; }

        /// <summary>
        /// Статус после перехода
        /// </summary>
        public int StatusIdEnd { get; set; }

        /// <summary>
        /// Пользователь, установивший текущий статус
        /// </summary>
        public string StatusUser { get; set; }

        /// <summary>
        /// Дата время перехода в StatusIdEnd
        /// </summary>
        public DateTimeOffset? StatusDate { get; set; }

        /// <summary>
        /// Комментарий 
        /// </summary>
        public string Comment { get; set; }

        public string StatusIdBeginningName => (StatusIdBeginning > 0) ? LookupTablesProxy.Instance.GetOrderStatus(StatusIdBeginning) : "";
        public string StatusIdEndName => (StatusIdEnd > 0) ? LookupTablesProxy.Instance.GetOrderStatus(StatusIdEnd) : "";
        public string StatusDateString => (StatusDate != null) ? ((DateTimeOffset)StatusDate).LocalDateTime.ToShortDateString() + " " +
                                                                 ((DateTimeOffset)StatusDate).LocalDateTime.ToShortTimeString() : "";

        public override void Merge(ObservableObject source)
        {
            if (source is OrderDishModel model)
            {
                Merge(model);
            }
        }

        public void Merge(OrderStatusHistoryModel source)
        {
            if (source != null)
            {
                OrderId = source.OrderId;
                OrderLine = source.OrderLine;
                StatusIdBeginning = source.StatusIdBeginning;
                StatusIdEnd = source.StatusIdEnd;
                StatusUser = source.StatusUser;
                StatusDate = source.StatusDate;
                Comment = source.Comment;
                //StatusIdBeginningName = source.StatusIdBeginningName;
                //StatusIdEndName = source.StatusIdEndName;
                //StatusDateString = source.StatusDateString;
            }
        }
    }
}
