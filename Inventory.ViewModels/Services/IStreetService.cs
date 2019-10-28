using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public interface IStreetService
    {
        Task<StreetModel> GetStreetAsync(int id);
        Task<IList<StreetModel>> GetStreetsAsync();
        Task<IList<StreetModel>> GetStreetsAsync(DataRequest<Street> request);
        StreetModel CreateStreetModel(Street entity);
    }
}
