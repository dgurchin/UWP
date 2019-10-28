using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class VariableService : IVariableService
    {
        public VariableService(IDataServiceFactory dataServiceFactory, ILogService logService)
        {
            DataServiceFactory = dataServiceFactory;
            LogService = logService;
        }

        public IDataServiceFactory DataServiceFactory { get; }

        public ILogService LogService { get; }

        public async Task<VariableModel> GetVariableAsync(int id)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return CreateModelFromEntity(await dataService.GetVariableAsync(id));
            }
        }

        public async Task<VariableModel> GetVariableByNameAsync(string name)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entity = await dataService.GetVariableAsync(name);
                return CreateModelFromEntity(entity);
            }
        }

        public async Task<IList<VariableModel>> GetVariablesAsync()
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                var entities = await dataService.GetVariablesAsync();
                var models = new List<VariableModel>();
                foreach (var entity in entities)
                {
                    models.Add(CreateModelFromEntity(entity));
                }
                return models;
            }
        }

        public async Task<int> UpdateVariableAsync(VariableModel variableModel)
        {
            using (var dataService = DataServiceFactory.CreateDataService())
            {
                return await dataService.UpdateVariableAsync(CreateEntityFromModel(variableModel));
            }
        }

        private VariableModel CreateModelFromEntity(Variable variable)
        {
            if (variable == null)
            {
                return new VariableModel();
            }

            return new VariableModel
            {
                Id = variable.Id,
                Name = variable.Name,
                Data = variable.Data,
                Description = variable.Description
            };
        }

        private Variable CreateEntityFromModel(VariableModel variable)
        {
            if (variable == null)
            {
                return new Variable();
            }

            return new Variable
            {
                Id = variable.Id,
                Name = variable.Name,
                Data = variable.Data,
                Description = variable.Description
            };
        }

        public async Task<T> GetVariableValueAsync<T>(int id, T defaultValue = default(T))
        {
            var variable = await GetVariableAsync(id);
            return GetVariableValue(variable, defaultValue);
        }

        public async Task<T> GetVariableValueAsync<T>(string name, T defaultValue = default(T))
        {
            var variable = await GetVariableByNameAsync(name);
            return GetVariableValue(variable, defaultValue);
        }

        public T GetVariableValue<T>(VariableModel variable, T defaultValue = default(T))
        {
            if (variable == null || string.IsNullOrEmpty(variable.Data))
                return defaultValue;

            try
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
                object value = typeConverter.ConvertFromString(variable.Data);
                return (T)value;
            }
            catch (System.NotSupportedException ex)
            {
                LogService.WriteAsync(LogType.Error, nameof(VariableService), nameof(GetVariableValue), ex);
                return defaultValue;
            }
        }
    }
}
