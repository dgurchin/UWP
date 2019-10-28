using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Data.Services
{
    partial class DataServiceBase
    {
        public async Task<Street> GetStreetAsync(int? id)
        {
            return await _dataSource.Streets
                .Include(x => x.StreetType)
                .Include(x => x.City)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<Street>> GetStreetsAsync()
        {
            return await _dataSource.Streets
                .Include(x => x.StreetType)
                .Include(x => x.City)
                .ToListAsync();
        }

        public async Task<IList<Street>> GetStreetsAsync(DataRequest<Street> request)
        {
            IQueryable<Street> items = GetStreets(request);

            // Execute
            var records = await items
                .AsNoTracking()
                .ToListAsync();

            return records;
        }

        private IQueryable<Street> GetStreets(DataRequest<Street> request)
        {
            IQueryable<Street> items = _dataSource.Streets
                .Include(street => street.StreetType)
                .Include(street => street.City);

            // Query
            if (!string.IsNullOrEmpty(request.Query))
            {
                // TODO: Оптимизировать запрос. Не выбирается в SQL.
                string query = request.Query.Replace(".", "").Replace("-", "").Trim();
                if (!string.IsNullOrEmpty(query))
                {
                    items = items.Where(street => $"{street.StreetType.NameShort.Replace(".", "").Replace("-", "")} {street.Name}".ToLower().Contains(query.ToLower()));
                }
            }

            // Where
            if (request.Where != null)
            {
                items = items.Where(request.Where);
            }

            // Order By
            if (request.OrderBy != null)
            {
                items = items.OrderBy(request.OrderBy);
            }
            if (request.OrderByDesc != null)
            {
                items = items.OrderByDescending(request.OrderByDesc);
            }

            return items;
        }
    }
}
