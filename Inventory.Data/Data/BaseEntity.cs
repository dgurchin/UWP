using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }

        /// <summary>
        /// Проверяет значения всех полей <see cref="int?"/> на 0 и переписывает их в <see cref="null"/> для избежания ошибки в БД с внешними ключами.
        /// </summary>
        public void ConvertNullableZero()
        {
            foreach (var info in GetType().GetProperties())
            {
                var type = info.PropertyType;
                var underlyingType = Nullable.GetUnderlyingType(type);
                if (underlyingType != null)
                {
                    var value = (int?)info.GetValue(this);
                    if (value <= 0)
                    {
                        info.SetValue(this, null);
                    }
                }
            }
        }
    }
}
