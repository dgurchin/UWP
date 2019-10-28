using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Models;

namespace Inventory.Services
{
    public interface IOrderDishModifierService
    {
        Task<IList<ModifierModel>> GetRelatedDishModifiersAsync(Guid dishGuid);
    }
}
