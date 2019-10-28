using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Services
{
    public interface IVariableService
    {
        /// <summary>
        /// Получить настройку по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<VariableModel> GetVariableAsync(int id);

        /// <summary>
        /// Получить настройку по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<VariableModel> GetVariableByNameAsync(string name);

        /// <summary>
        /// Получить все настройки
        /// </summary>
        /// <returns></returns>
        Task<IList<VariableModel>> GetVariablesAsync();

        /// <summary>
        /// Получить значение по id
        /// </summary>
        /// <typeparam name="T">Тип для конвертации значения</typeparam>
        /// <param name="id">Id настройки</param>
        /// <param name="defaultValue">Значение настройки по умолчанию если настройку не найдет</param>
        /// <returns></returns>
        Task<T> GetVariableValueAsync<T>(int id, T defaultValue = default(T));

        /// <summary>
        /// Получить значение по имени
        /// </summary>
        /// <typeparam name="T">Тип для конвертации значения</typeparam>
        /// <param name="name">Имя настройки <see cref="Models.Enums.VariableString"/></param>
        /// <param name="defaultValue">Значение настройки по умолчанию если настройку не найдет</param>
        /// <returns></returns>
        Task<T> GetVariableValueAsync<T>(string name, T defaultValue = default(T));

        /// <summary>
        /// Получить значение настройки с определенным типом
        /// </summary>
        /// <typeparam name="T">Тип для конвертации значения</typeparam>
        /// <param name="variable">Модель настройки</param>
        /// <param name="defaultValue">Значение настройки по умолчанию если настройка null</param>
        T GetVariableValue<T>(VariableModel variable, T defaultValue = default(T));

        /// <summary>
        /// Обновляет или добавляет настройку
        /// </summary>
        /// <param name="variableModel"></param>
        /// <returns></returns>
        Task<int> UpdateVariableAsync(VariableModel variableModel);
    }
}
