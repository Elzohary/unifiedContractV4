using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.WorkOrder;

namespace UnifiedContract.Domain.Interfaces.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetItemsByClientIdAsync(Guid clientId);
        Task<IEnumerable<Item>> GetActiveItemsByClientIdAsync(Guid clientId);
        Task<IEnumerable<Item>> GetActiveItemsAsync();
        Task<Item> GetItemByNumberAsync(string itemNumber, Guid clientId);
        Task<bool> ItemNumberExistsAsync(string itemNumber, Guid clientId, Guid? excludeId = null);
    }
} 