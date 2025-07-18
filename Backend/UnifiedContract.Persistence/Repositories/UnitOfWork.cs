using System;
using System.Collections;
using System.Threading.Tasks;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Interfaces.Repositories;

namespace UnifiedContract.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UnifiedContractDbContext _context;
        private Hashtable _repositories = new Hashtable();
        private bool _disposed;

        public UnitOfWork(UnifiedContractDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IRepository<TEntity>)_repositories[type]!;
        }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task<bool> SaveEntitiesAsync()
        {
            // Dispatch Domain Events
            
            // Save changes to the database
            await _context.SaveChangesAsync();
            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
} 