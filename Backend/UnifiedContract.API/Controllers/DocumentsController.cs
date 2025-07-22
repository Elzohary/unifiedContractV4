using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnifiedContract.Application.Interfaces;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Domain.Interfaces.Repositories;
using System.Linq;
using System.Collections.Generic;
using UnifiedContract.API.Models;

namespace UnifiedContract.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly IRepository<Attachment> _attachmentRepository;

        public DocumentsController(IFileStorageService fileStorageService, IRepository<Attachment> attachmentRepository)
        {
            _fileStorageService = fileStorageService;
            _attachmentRepository = attachmentRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] Guid entityId, [FromForm] string entityType)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            if (entityId == Guid.Empty || string.IsNullOrEmpty(entityType))
                return BadRequest("Entity ID and Entity Type are required.");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized();
            }

            var filePath = await _fileStorageService.SaveFileAsync(file.OpenReadStream(), file.FileName, file.ContentType);

            var attachment = new Attachment
            {
                Id = Guid.NewGuid(),
                FileName = file.FileName,
                FilePath = filePath,
                FileType = file.ContentType,
                FileSize = file.Length,
                EntityId = entityId,
                EntityType = entityType,
                UploadedById = userId,
                UploadDate = DateTime.UtcNow,
                CreatedBy = User.Identity?.Name ?? "System",
                CreatedAt = DateTime.UtcNow
            };

            await _attachmentRepository.AddAsync(attachment);

            return Ok(new ApiResponse<object> 
            { 
                Status = true, 
                Message = "File uploaded successfully.", 
                Data = new { FilePath = filePath, Id = attachment.Id } 
            });
        }

        [HttpGet("{entityType}/{entityId}")]
        public async Task<IActionResult> GetDocuments(string entityType, Guid entityId)
        {
            var attachments = await _attachmentRepository.GetAllAsync();
            var filteredAttachments = attachments.Where(a => a.EntityType == entityType && a.EntityId == entityId).ToList();
            
            return Ok(new ApiResponse<IEnumerable<Attachment>> 
            { 
                Status = true, 
                Message = "Documents retrieved successfully.", 
                Data = filteredAttachments 
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(Guid id)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(id);
            if (attachment == null)
            {
                return NotFound(new ApiResponse<object> { Status = false, Message = "Attachment not found." });
            }

            await _attachmentRepository.DeleteAsync(attachment);
            await _fileStorageService.DeleteFileAsync(attachment.FilePath);

            return Ok(new ApiResponse<object> { Status = true, Message = "Attachment deleted successfully." });
        }
    }
} 