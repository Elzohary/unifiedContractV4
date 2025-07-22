using UnifiedContract.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace UnifiedContract.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UnifiedContractDbContext _dbContext;

        public UnitOfWork(UnifiedContractDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
} 