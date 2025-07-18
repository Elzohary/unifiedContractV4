using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Application.Interfaces;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Persistence.Repositories
{
    public class ClientMaterialRepository : Repository<ClientMaterial>, IClientMaterialRepository
    {
        private readonly UnifiedContractDbContext _dbContext;

        public ClientMaterialRepository(UnifiedContractDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<ClientMaterial>> GetMaterialsByClientAsync(Guid clientId)
        {
            return await _dbContext.ClientMaterials
                .Where(m => m.ClientId == clientId)
                .OrderBy(m => m.GroupCode)
                .ThenBy(m => m.SEQ)
                .Include(m => m.Client)
                .ToListAsync();
        }

        public async Task<ClientMaterial> GetByMaterialCodeAsync(string materialCode, Guid clientId)
        {
            return await _dbContext.ClientMaterials
                .Where(m => m.MaterialMasterCode == materialCode && m.ClientId == clientId)
                .Include(m => m.Client)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsMaterialCodeUniqueAsync(string materialCode, Guid clientId, Guid? excludeId = null)
        {
            var query = _dbContext.ClientMaterials
                .Where(m => m.MaterialMasterCode == materialCode && m.ClientId == clientId);

            if (excludeId.HasValue)
            {
                query = query.Where(m => m.Id != excludeId.Value);
            }

            return await query.CountAsync() == 0;
        }
    }
} 