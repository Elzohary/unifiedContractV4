using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnifiedContract.Application.DTOs;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Interfaces.Repositories;
using System.Linq;

namespace UnifiedContract.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/work-orders/{workOrderId}/permits")]
    public class PermitController : ControllerBase
    {
        private readonly IWorkOrderRepository _workOrderRepository;

        public PermitController(IWorkOrderRepository workOrderRepository)
        {
            _workOrderRepository = workOrderRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWorkOrderPermits(Guid workOrderId, [FromBody] List<PermitDto> permits)
        {
            var currentUser = User.Identity?.Name ?? "system";
            var newPermits = permits.Select(p => new Permit
            {
                Type = p.Type,
                Status = Enum.Parse<UnifiedContract.Domain.Enums.PermitStatus>(p.Status, true),
                Title = p.Type, // Default
                IssueDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                Authority = "",
                Description = "",
                Number = "",
                IssuedBy = "",
                DocumentRef = "",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser
            });

            await _workOrderRepository.UpdateWorkOrderPermitsAsync(workOrderId, newPermits, currentUser);

            return NoContent();
        }
    }
} 