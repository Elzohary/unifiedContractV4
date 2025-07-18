using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Interfaces.Repositories;

namespace UnifiedContract.Application.Interfaces
{
    public interface IClientMaterialRepository : IRepository<ClientMaterial>
    {
        Task<IEnumerable<ClientMaterial>> GetMaterialsByClientAsync(Guid clientId);
        Task<ClientMaterial> GetByMaterialCodeAsync(string materialCode, Guid clientId);
        Task<bool> IsMaterialCodeUniqueAsync(string materialCode, Guid clientId, Guid? excludeId = null);
    }
} 