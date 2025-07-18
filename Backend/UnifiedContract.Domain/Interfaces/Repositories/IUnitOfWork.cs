using System;
using System.Threading.Tasks;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();
        Task<bool> SaveEntitiesAsync();
    }
} 