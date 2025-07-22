using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using UnifiedContract.Application.Interfaces;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace UnifiedContract.Infrastructure.Services
{
    public class LocalStorageService : IFileStorageService
    {
        private readonly string _storagePath;
        private readonly string _basePathForUrls;

        public LocalStorageService(IConfiguration configuration, IWebHostEnvironment env)
        {
            _basePathForUrls = "uploads";
            var configuredPath = configuration.GetValue<string>("StorageSettings:LocalStoragePath");

            if (!string.IsNullOrEmpty(configuredPath))
            {
                _storagePath = Path.IsPathRooted(configuredPath)
                    ? configuredPath
                    : Path.GetFullPath(Path.Combine(env.ContentRootPath, configuredPath));
            }
            else
            {
                _storagePath = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"), _basePathForUrls);
            }
            
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var filePath = Path.Combine(_storagePath, uniqueFileName);

            fileStream.Position = 0;
            await using var stream = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(stream, cancellationToken);

            return Path.Combine(_basePathForUrls, uniqueFileName).Replace('\\', '/');
        }

        public Task DeleteFileAsync(string relativePath, CancellationToken cancellationToken = default)
        {
            var fileName = Path.GetFileName(relativePath);
            var filePath = Path.Combine(_storagePath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }
} 