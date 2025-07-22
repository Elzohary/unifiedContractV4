using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnifiedContract.API.Models;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Interfaces.Repositories;
using UnifiedContract.Domain.Interfaces;
using UnifiedContract.Application.DTOs;
using System.Linq;

namespace UnifiedContract.API.Controllers
{
    [Route("api/work-orders")]
    [ApiController]
    [Authorize]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderRepository _workOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkOrdersController(IWorkOrderRepository workOrderRepository, IUnitOfWork unitOfWork)
        {
            _workOrderRepository = workOrderRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/WorkOrders
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkOrderListItemDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkOrderListItemDto>>>> GetWorkOrders()
        {
            var workOrders = await _workOrderRepository.GetAllAsyncWithStatusPriorityClient();
            var dtos = workOrders.Select(wo => new WorkOrderListItemDto
            {
                Id = wo.Id,
                WorkOrderNumber = wo.WorkOrderNumber,
                InternalOrderNumber = wo.InternalOrderNumber,
                Title = wo.Title,
                Description = wo.Description,
                Client = wo.Client?.Name ?? string.Empty,
                Location = wo.Location,
                StatusCode = wo.Status?.Code ?? string.Empty,
                StatusName = wo.Status?.Name ?? string.Empty,
                PriorityCode = wo.Priority?.Code ?? string.Empty,
                PriorityName = wo.Priority?.Name ?? string.Empty,
                Category = wo.Category,
                CompletionPercentage = wo.CompletionPercentage,
                ReceivedDate = wo.ReceivedDate,
                StartDate = wo.StartDate,
                DueDate = wo.DueDate,
                TargetEndDate = wo.TargetEndDate,
                CreatedAt = wo.CreatedAt,
                CreatedBy = wo.CreatedBy,
                LastModifiedAt = wo.LastModifiedAt,
                EstimatedCost = wo.EstimatedCost
            });
            return Ok(new ApiResponse<IEnumerable<WorkOrderListItemDto>>
            {
                Status = true,
                Data = dtos,
                Message = "Success"
            });
        }

        // GET: api/WorkOrders/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<WorkOrderDetailsDto>>> GetWorkOrder(Guid id)
        {
            var workOrder = await _workOrderRepository.GetWorkOrderWithDetailsAsync(id);

            if (workOrder == null)
            {
                return NotFound(new ApiResponse<WorkOrderDetailsDto>
                {
                    Status = false,
                    Message = "Work order not found"
                });
            }

            var dto = new UnifiedContract.Application.DTOs.WorkOrderDetailsDto
            {
                Id = workOrder.Id,
                WorkOrderNumber = workOrder.WorkOrderNumber,
                InternalOrderNumber = workOrder.InternalOrderNumber,
                Title = workOrder.Title,
                Description = workOrder.Description,
                Client = workOrder.Client?.Name ?? string.Empty,
                Location = workOrder.Location,
                Status = workOrder.Status?.Name ?? workOrder.Status?.Code ?? string.Empty,
                Priority = workOrder.Priority?.Name ?? workOrder.Priority?.Code ?? string.Empty,
                Category = workOrder.Category,
                CompletionPercentage = workOrder.CompletionPercentage,
                ReceivedDate = workOrder.ReceivedDate,
                StartDate = workOrder.StartDate,
                DueDate = workOrder.DueDate,
                TargetEndDate = workOrder.TargetEndDate,
                CreatedDate = workOrder.CreatedAt,
                CreatedBy = workOrder.CreatedBy,
                LastUpdated = workOrder.LastModifiedAt,
                EstimatedPrice = workOrder.EstimatedCost,
                EngineerInCharge = workOrder.EngineerInCharge?.FullName ?? string.Empty,
                Type = workOrder.Type ?? string.Empty,
                Class = workOrder.Class ?? string.Empty,
                Permits = workOrder.Permits.Select(p => new Application.DTOs.PermitDto { Type = p.Type, Status = p.Status.ToString() }).ToList()
            };

            return Ok(new ApiResponse<UnifiedContract.Application.DTOs.WorkOrderDetailsDto>
            {
                Status = true,
                Data = dto,
                Message = "Success"
            });
        }

        // POST: api/WorkOrders
        [HttpPost]
        [ProducesResponseType(typeof(WorkOrder), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<WorkOrder>> CreateWorkOrder([FromBody] WorkOrder workOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            workOrder.CreatedBy = User.Identity.Name;
            var createdWorkOrder = await _workOrderRepository.AddAsync(workOrder);

            return CreatedAtAction(nameof(GetWorkOrder), new { id = createdWorkOrder.Id }, createdWorkOrder);
        }

        // PUT: api/WorkOrders/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateWorkOrder(Guid id, [FromBody] WorkOrder workOrder)
        {
            if (id != workOrder.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingWorkOrder = await _workOrderRepository.GetByIdAsync(id);
            if (existingWorkOrder == null)
            {
                return NotFound();
            }

            workOrder.LastModifiedBy = User.Identity.Name;
            await _workOrderRepository.UpdateAsync(workOrder);

            return NoContent();
        }

        // DELETE: api/WorkOrders/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteWorkOrder(Guid id)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(id);
            if (workOrder == null)
            {
                return NotFound();
            }

            await _workOrderRepository.DeleteAsync(workOrder);

            return NoContent();
        }

        // GET: api/WorkOrders/client/{client}
        [HttpGet("client/{client}")]
        [ProducesResponseType(typeof(IEnumerable<WorkOrder>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetWorkOrdersByClient(string client)
        {
            var workOrders = await _workOrderRepository.GetWorkOrdersByClientAsync(client);
            return Ok(workOrders);
        }

        // GET: api/WorkOrders/status/{status}
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<WorkOrder>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetWorkOrdersByStatus(string status)
        {
            var workOrders = await _workOrderRepository.GetWorkOrdersWithStatusAsync(status);
            return Ok(workOrders);
        }

        // GET: api/WorkOrders/{id}/items
        [HttpGet("{id}/items")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkOrderItem>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkOrderItem>>>> GetWorkOrderItems(Guid id)
        {
            var items = await _workOrderRepository.GetItemsByWorkOrderIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<WorkOrderItem>>
            {
                Status = true,
                Data = items,
                Message = "Success"
            });
        }

        // POST: api/WorkOrders/{id}/items
        [HttpPost("{id}/items")]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderItem>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<WorkOrderItem>>> AddWorkOrderItem(Guid id, [FromBody] WorkOrderItem item)
        {
            if (item == null)
            {
                return BadRequest(new ApiResponse<WorkOrderItem> { Status = false, Message = "Item is required." });
            }
            item.WorkOrderId = id;
            item.Id = Guid.NewGuid();
            item.CreatedAt = DateTime.UtcNow;
            item.CreatedBy = User?.Identity?.Name ?? "system";
            await _workOrderRepository.AddItemAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWorkOrderItems), new { id }, new ApiResponse<WorkOrderItem>
            {
                Status = true,
                Data = item,
                Message = "Item assigned to work order."
            });
        }

        // GET: api/work-order-items
        [HttpGet("/api/work-order-items")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkOrderItem>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkOrderItem>>>> GetAllWorkOrderItems()
        {
            var items = await _workOrderRepository.GetAllItemsAsync();
            return Ok(new ApiResponse<IEnumerable<WorkOrderItem>>
            {
                Status = true,
                Data = items,
                Message = "Success"
            });
        }

        // POST: api/work-order-items
        [HttpPost("/api/work-order-items")]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderItem>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<WorkOrderItem>>> CreateWorkOrderItem([FromBody] CreateWorkOrderItemDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ApiResponse<WorkOrderItem> { Status = false, Message = "Item is required." });
            }
            var item = new WorkOrderItem
            {
                Id = Guid.NewGuid(),
                ItemNumber = dto.ItemNumber,
                Description = dto.Description,
                Unit = dto.Unit,
                UnitPrice = dto.UnitPrice,
                PaymentType = dto.PaymentType,
                ManagementArea = dto.ManagementArea,
                Currency = dto.Currency,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = User?.Identity?.Name ?? "system",
                LastModifiedBy = User?.Identity?.Name ?? "system",
                LastModifiedAt = DateTime.UtcNow,
                ReasonForFinalQuantity = string.Empty
            };
            await _workOrderRepository.AddItemAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllWorkOrderItems), null, new ApiResponse<WorkOrderItem>
            {
                Status = true,
                Data = item,
                Message = "Item created."
            });
        }
    }
} 