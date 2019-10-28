using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<Variable> GetVariableAsync(int id)
        {
            return await _dataSource.Variables.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Variable> GetVariableAsync(string name)
        {
            return await _dataSource.Variables.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IList<Variable>> GetVariablesAsync()
        {
            return await _dataSource.Variables.ToListAsync();
        }

        public async Task<int> UpdateVariableAsync(Variable variable)
        {
            if (variable.Id > 0)
            {
                _dataSource.Entry(variable).State = EntityState.Modified;
            }
            else
            {
                _dataSource.Entry(variable).State = EntityState.Added;
            }
            return await _dataSource.SaveChangesAsync();
        }
    }
}
