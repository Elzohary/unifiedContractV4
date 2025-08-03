using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Interfaces.Repositories;
using UnifiedContract.Persistence;

namespace UnifiedContract.Persistence.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(UnifiedContractDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Item>> GetItemsByClientIdAsync(Guid clientId)
        {
            return await _dbContext.Items
                .Where(item => item.ClientId == clientId)
                .Include(item => item.Client)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetActiveItemsByClientIdAsync(Guid clientId)
        {
            return await _dbContext.Items
                .Where(item => item.ClientId == clientId && item.IsActive)
                .Include(item => item.Client)
                .ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetActiveItemsAsync()
        {
            return await _dbContext.Items
                .Where(item => item.IsActive)
                .Include(item => item.Client)
                .ToListAsync();
        }

        public async Task<Item> GetItemByNumberAsync(string itemNumber, Guid clientId)
        {
            return await _dbContext.Items
                .FirstOrDefaultAsync(item => item.ItemNumber == itemNumber && item.ClientId == clientId);
        }

        public async Task<bool> ItemNumberExistsAsync(string itemNumber, Guid clientId, Guid? excludeId = null)
        {
            var query = _dbContext.Items
                .Where(item => item.ItemNumber == itemNumber && item.ClientId == clientId);

            if (excludeId.HasValue)
            {
                query = query.Where(item => item.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
} 