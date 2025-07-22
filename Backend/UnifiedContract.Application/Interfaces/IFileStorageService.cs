using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace UnifiedContract.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default);
        Task DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
    }
} 