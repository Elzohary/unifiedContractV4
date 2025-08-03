using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnifiedContract.API.Models;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Interfaces.Repositories;
using UnifiedContract.Domain.Interfaces;
using UnifiedContract.Application.DTOs;
using System.Linq;
using System;

namespace UnifiedContract.API.Controllers
{
    [Route("api/work-orders")]
    [ApiController]
    [Authorize]
    public class WorkOrdersController : ControllerBase
    {
        private readonly IWorkOrderRepository _workOrderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IWorkOrderItemAssignmentRepository _workOrderItemAssignmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkOrdersController(
            IWorkOrderRepository workOrderRepository, 
            IItemRepository itemRepository,
            IWorkOrderItemAssignmentRepository workOrderItemAssignmentRepository,
            IUnitOfWork unitOfWork)
        {
            _workOrderRepository = workOrderRepository;
            _itemRepository = itemRepository;
            _workOrderItemAssignmentRepository = workOrderItemAssignmentRepository;
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
                Status = 200,
                Data = dtos,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
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
                    Status = 404,
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
                ProjectType = workOrder.ProjectType ?? string.Empty,
                PO = workOrder.PO ?? string.Empty,
                D1 = workOrder.D1 ?? string.Empty,
                Permits = workOrder.Permits.Select(p => new Application.DTOs.PermitDto { Type = p.Type, Status = p.Status.ToString() }).ToList(),
                ItemAssignments = workOrder.ItemAssignments?.Select(assignment => new Application.DTOs.WorkOrderItemAssignmentDto
                {
                    Id = assignment.Id,
                    WorkOrderId = assignment.WorkOrderId,
                    ItemId = assignment.ItemId,
                    EstimatedQuantity = assignment.EstimatedQuantity,
                    EstimatedPrice = assignment.EstimatedPrice,
                    EstimatedPriceWithVAT = assignment.EstimatedPriceWithVAT,
                    ActualQuantity = assignment.ActualQuantity,
                    ActualPrice = assignment.ActualPrice,
                    ActualPriceWithVAT = assignment.ActualPriceWithVAT,
                    ReasonForFinalQuantity = assignment.ReasonForFinalQuantity,
                    CreatedAt = assignment.CreatedAt,
                    CreatedBy = assignment.CreatedBy,
                    LastModifiedAt = assignment.LastModifiedAt,
                    LastModifiedBy = assignment.LastModifiedBy,
                    Item = assignment.Item != null ? new Application.DTOs.ItemDto
                    {
                        Id = assignment.Item.Id,
                        ItemNumber = assignment.Item.ItemNumber,
                        Description = assignment.Item.Description,
                        Unit = assignment.Item.Unit,
                        UnitPrice = assignment.Item.UnitPrice,
                        PaymentType = assignment.Item.PaymentType,
                        ManagementArea = assignment.Item.ManagementArea,
                        Currency = assignment.Item.Currency,
                        IsActive = assignment.Item.IsActive,
                        ClientId = assignment.Item.ClientId,
                        ClientName = assignment.Item.Client?.Name ?? string.Empty,
                        CreatedAt = assignment.Item.CreatedAt,
                        CreatedBy = assignment.Item.CreatedBy,
                        LastModifiedAt = assignment.Item.LastModifiedAt,
                        LastModifiedBy = assignment.Item.LastModifiedBy
                    } : null
                }).ToList() ?? new List<Application.DTOs.WorkOrderItemAssignmentDto>()
            };

            return Ok(new ApiResponse<UnifiedContract.Application.DTOs.WorkOrderDetailsDto>
            {
                Status = 200,
                Data = dto,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // POST: api/WorkOrders
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderDetailsDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<WorkOrderDetailsDto>>> CreateWorkOrder([FromBody] CreateWorkOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<WorkOrderDetailsDto>
                {
                    Status = 400,
                    Message = "Invalid request data",
                    ValidationErrors = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    )
                });
            }

            try
            {
                var workOrder = new WorkOrder
                {
                    Id = Guid.NewGuid(),
                    WorkOrderNumber = dto.WorkOrderNumber,
                    InternalOrderNumber = dto.InternalOrderNumber,
                    Title = dto.Title,
                    Description = dto.Description,
                    Location = dto.Location,
                    Category = dto.Category,
                    Type = dto.Type,
                    Class = dto.Class,
                    ProjectType = dto.ProjectType,
                    PO = dto.PO,
                    D1 = dto.D1,
                    CompletionPercentage = dto.CompletionPercentage,
                    ReceivedDate = dto.ReceivedDate,
                    StartDate = dto.StartDate,
                    DueDate = dto.DueDate,
                    TargetEndDate = dto.TargetEndDate,
                    EstimatedCost = dto.EstimatedCost,
                    WorkOrderStatusId = dto.WorkOrderStatusId,
                    PriorityLevelId = dto.PriorityLevelId,
                    ClientId = dto.ClientId,
                    EngineerInChargeId = dto.EngineerInChargeId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = User?.Identity?.Name ?? "system",
                    LastModifiedAt = DateTime.UtcNow,
                    LastModifiedBy = User?.Identity?.Name ?? "system"
                };

            var createdWorkOrder = await _workOrderRepository.AddAsync(workOrder);
                await _unitOfWork.SaveChangesAsync();

                // Get the created work order with details
                var workOrderWithDetails = await _workOrderRepository.GetWorkOrderWithDetailsAsync(createdWorkOrder.Id);

                var workOrderDto = new WorkOrderDetailsDto
                {
                    Id = workOrderWithDetails.Id,
                    WorkOrderNumber = workOrderWithDetails.WorkOrderNumber,
                    InternalOrderNumber = workOrderWithDetails.InternalOrderNumber,
                    Title = workOrderWithDetails.Title,
                    Description = workOrderWithDetails.Description,
                    Client = workOrderWithDetails.Client?.Name ?? string.Empty,
                    Location = workOrderWithDetails.Location,
                    Status = workOrderWithDetails.Status?.Name ?? workOrderWithDetails.Status?.Code ?? string.Empty,
                    Priority = workOrderWithDetails.Priority?.Name ?? workOrderWithDetails.Priority?.Code ?? string.Empty,
                    Category = workOrderWithDetails.Category,
                    CompletionPercentage = workOrderWithDetails.CompletionPercentage,
                    ReceivedDate = workOrderWithDetails.ReceivedDate,
                    StartDate = workOrderWithDetails.StartDate,
                    DueDate = workOrderWithDetails.DueDate,
                    TargetEndDate = workOrderWithDetails.TargetEndDate,
                    CreatedDate = workOrderWithDetails.CreatedAt,
                    CreatedBy = workOrderWithDetails.CreatedBy,
                    LastUpdated = workOrderWithDetails.LastModifiedAt,
                    EstimatedPrice = workOrderWithDetails.EstimatedCost,
                    EngineerInCharge = workOrderWithDetails.EngineerInCharge?.FullName ?? string.Empty,
                    Type = workOrderWithDetails.Type ?? string.Empty,
                    Class = workOrderWithDetails.Class ?? string.Empty,
                    ProjectType = workOrderWithDetails.ProjectType ?? string.Empty,
                    PO = workOrderWithDetails.PO ?? string.Empty,
                    D1 = workOrderWithDetails.D1 ?? string.Empty,
                    Permits = workOrderWithDetails.Permits.Select(p => new Application.DTOs.PermitDto { Type = p.Type, Status = p.Status.ToString() }).ToList(),
                    ItemAssignments = workOrderWithDetails.ItemAssignments?.Select(assignment => new Application.DTOs.WorkOrderItemAssignmentDto
                    {
                        Id = assignment.Id,
                        WorkOrderId = assignment.WorkOrderId,
                        ItemId = assignment.ItemId,
                        EstimatedQuantity = assignment.EstimatedQuantity,
                        EstimatedPrice = assignment.EstimatedPrice,
                        EstimatedPriceWithVAT = assignment.EstimatedPriceWithVAT,
                        ActualQuantity = assignment.ActualQuantity,
                        ActualPrice = assignment.ActualPrice,
                        ActualPriceWithVAT = assignment.ActualPriceWithVAT,
                        ReasonForFinalQuantity = assignment.ReasonForFinalQuantity,
                        CreatedAt = assignment.CreatedAt,
                        CreatedBy = assignment.CreatedBy,
                        LastModifiedAt = assignment.LastModifiedAt,
                        LastModifiedBy = assignment.LastModifiedBy,
                        Item = assignment.Item != null ? new Application.DTOs.ItemDto
                        {
                            Id = assignment.Item.Id,
                            ItemNumber = assignment.Item.ItemNumber,
                            Description = assignment.Item.Description,
                            Unit = assignment.Item.Unit,
                            UnitPrice = assignment.Item.UnitPrice,
                            PaymentType = assignment.Item.PaymentType,
                            ManagementArea = assignment.Item.ManagementArea,
                            Currency = assignment.Item.Currency,
                            IsActive = assignment.Item.IsActive,
                            ClientId = assignment.Item.ClientId,
                            ClientName = assignment.Item.Client?.Name ?? string.Empty,
                            CreatedAt = assignment.Item.CreatedAt,
                            CreatedBy = assignment.Item.CreatedBy,
                            LastModifiedAt = assignment.Item.LastModifiedAt,
                            LastModifiedBy = assignment.Item.LastModifiedBy
                        } : null
                    }).ToList() ?? new List<Application.DTOs.WorkOrderItemAssignmentDto>()
                };

                return CreatedAtAction(nameof(GetWorkOrder), new { id = createdWorkOrder.Id }, new ApiResponse<WorkOrderDetailsDto>
                {
                    Status = 201,
                    Data = workOrderDto,
                    Message = "Work order created successfully",
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<WorkOrderDetailsDto>
                {
                    Status = 400,
                    Message = $"Error creating work order: {ex.Message}"
                });
            }
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
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteWorkOrder(Guid id)
        {
            try
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(id);
            if (workOrder == null)
                {
                    return NotFound(new ApiResponse<bool>
                    {
                        Status = 404,
                        Message = "Work order not found",
                        Data = false
                    });
                }

                // Delete all related data first
                // 1. Delete item assignments
                var itemAssignments = await _workOrderItemAssignmentRepository.GetAssignmentsByWorkOrderIdAsync(id);
                foreach (var assignment in itemAssignments)
                {
                    await _workOrderItemAssignmentRepository.DeleteAsync(assignment);
                }

                // 2. Delete the work order itself
                await _workOrderRepository.DeleteAsync(workOrder);

                // 3. Save all changes
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ApiResponse<bool>
                {
                    Status = 200,
                    Message = "Work order and all related data deleted successfully",
                    Data = true,
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<bool>
                {
                    Status = 500,
                    Message = $"Error deleting work order: {ex.Message}",
                    Data = false,
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                });
            }
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
                Status = 200,
                Data = items,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // GET: api/work-orders/{id}/item-assignments
        [HttpGet("{id}/item-assignments")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkOrderItemAssignmentDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkOrderItemAssignmentDto>>>> GetWorkOrderItemAssignments(Guid id)
        {
            var assignments = await _workOrderItemAssignmentRepository.GetAssignmentsByWorkOrderIdAsync(id);

            var assignmentDtos = assignments.Select(assignment => new Application.DTOs.WorkOrderItemAssignmentDto
            {
                Id = assignment.Id,
                WorkOrderId = assignment.WorkOrderId,
                ItemId = assignment.ItemId,
                EstimatedQuantity = assignment.EstimatedQuantity,
                EstimatedPrice = assignment.EstimatedPrice,
                EstimatedPriceWithVAT = assignment.EstimatedPriceWithVAT,
                ActualQuantity = assignment.ActualQuantity,
                ActualPrice = assignment.ActualPrice,
                ActualPriceWithVAT = assignment.ActualPriceWithVAT,
                ReasonForFinalQuantity = assignment.ReasonForFinalQuantity,
                CreatedAt = assignment.CreatedAt,
                CreatedBy = assignment.CreatedBy,
                LastModifiedAt = assignment.LastModifiedAt,
                LastModifiedBy = assignment.LastModifiedBy,
                Item = assignment.Item != null ? new Application.DTOs.ItemDto
                {
                    Id = assignment.Item.Id,
                    ItemNumber = assignment.Item.ItemNumber,
                    Description = assignment.Item.Description,
                    Unit = assignment.Item.Unit,
                    UnitPrice = assignment.Item.UnitPrice,
                    PaymentType = assignment.Item.PaymentType,
                    ManagementArea = assignment.Item.ManagementArea,
                    Currency = assignment.Item.Currency,
                    IsActive = assignment.Item.IsActive,
                    ClientId = assignment.Item.ClientId,
                    ClientName = assignment.Item.Client?.Name ?? string.Empty,
                    CreatedAt = assignment.Item.CreatedAt,
                    CreatedBy = assignment.Item.CreatedBy,
                    LastModifiedAt = assignment.Item.LastModifiedAt,
                    LastModifiedBy = assignment.Item.LastModifiedBy
                } : null
            }).ToList();

            return Ok(new ApiResponse<IEnumerable<Application.DTOs.WorkOrderItemAssignmentDto>>
            {
                Status = 200,
                Data = assignmentDtos,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // GET: api/work-orders/available-items
        [HttpGet("available-items")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ItemDto>>>> GetAllAvailableItems()
        {
            var availableItems = await _itemRepository.GetActiveItemsAsync();

            var itemDtos = availableItems.Select(item => new Application.DTOs.ItemDto
            {
                Id = item.Id,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                Unit = item.Unit,
                UnitPrice = item.UnitPrice,
                PaymentType = item.PaymentType,
                ManagementArea = item.ManagementArea,
                Currency = item.Currency,
                IsActive = item.IsActive,
                ClientId = item.ClientId,
                ClientName = item.Client?.Name ?? string.Empty,
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                LastModifiedAt = item.LastModifiedAt,
                LastModifiedBy = item.LastModifiedBy
            }).ToList();

            return Ok(new ApiResponse<IEnumerable<Application.DTOs.ItemDto>>
            {
                Status = 200,
                Data = itemDtos,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // POST: api/work-orders/master-items
        [HttpPost("master-items")]
        [ProducesResponseType(typeof(ApiResponse<ItemDto>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<ItemDto>>> CreateMasterItem([FromBody] CreateMasterItemDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ApiResponse<ItemDto> { Status = 400, Message = "Item data is required." });
            }

            // Check if item number already exists
            if (await _itemRepository.ItemNumberExistsAsync(dto.ItemNumber, dto.ClientId))
            {
                return BadRequest(new ApiResponse<ItemDto> { Status = 400, Message = "Item number already exists for this client." });
            }

            var item = new Domain.Entities.WorkOrder.Item
            {
                Id = Guid.NewGuid(),
                ItemNumber = dto.ItemNumber,
                Description = dto.Description,
                Unit = dto.Unit,
                UnitPrice = dto.UnitPrice,
                PaymentType = dto.PaymentType,
                ManagementArea = dto.ManagementArea,
                Currency = dto.Currency,
                IsActive = true,
                ClientId = dto.ClientId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = User?.Identity?.Name ?? "system",
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedBy = User?.Identity?.Name ?? "system"
            };

            await _itemRepository.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            var itemDto = new Application.DTOs.ItemDto
            {
                Id = item.Id,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                Unit = item.Unit,
                UnitPrice = item.UnitPrice,
                PaymentType = item.PaymentType,
                ManagementArea = item.ManagementArea,
                Currency = item.Currency,
                IsActive = item.IsActive,
                ClientId = item.ClientId,
                ClientName = string.Empty, // Will be populated if needed
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                LastModifiedAt = item.LastModifiedAt,
                LastModifiedBy = item.LastModifiedBy
            };

            return CreatedAtAction(nameof(GetAllAvailableItems), null, new ApiResponse<ItemDto>
            {
                Status = 201,
                Data = itemDto,
                Message = "Master item created successfully.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // PUT: api/work-orders/master-items/{id}
        [HttpPut("master-items/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ItemDto>>> UpdateMasterItem(Guid id, [FromBody] UpdateMasterItemDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ApiResponse<ItemDto> { Status = 400, Message = "Item data is required." });
            }

            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(new ApiResponse<ItemDto> { Status = 404, Message = "Item not found." });
            }

            // Check if item number already exists (excluding current item)
            if (await _itemRepository.ItemNumberExistsAsync(dto.ItemNumber, dto.ClientId, id))
            {
                return BadRequest(new ApiResponse<ItemDto> { Status = 400, Message = "Item number already exists for this client." });
            }

            // Update item properties
            item.ItemNumber = dto.ItemNumber;
            item.Description = dto.Description;
            item.Unit = dto.Unit;
            item.UnitPrice = dto.UnitPrice;
            item.PaymentType = dto.PaymentType;
            item.ManagementArea = dto.ManagementArea;
            item.Currency = dto.Currency;
            item.IsActive = dto.IsActive;
            item.ClientId = dto.ClientId;
            item.LastModifiedAt = DateTime.UtcNow;
            item.LastModifiedBy = User?.Identity?.Name ?? "system";

            await _itemRepository.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();

            var itemDto = new Application.DTOs.ItemDto
            {
                Id = item.Id,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                Unit = item.Unit,
                UnitPrice = item.UnitPrice,
                PaymentType = item.PaymentType,
                ManagementArea = item.ManagementArea,
                Currency = item.Currency,
                IsActive = item.IsActive,
                ClientId = item.ClientId,
                ClientName = string.Empty, // Will be populated if needed
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                LastModifiedAt = item.LastModifiedAt,
                LastModifiedBy = item.LastModifiedBy
            };

            return Ok(new ApiResponse<ItemDto>
            {
                Status = 200,
                Data = itemDto,
                Message = "Master item updated successfully.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // DELETE: api/work-orders/master-items/{id}
        [HttpDelete("master-items/{id}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMasterItem(Guid id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(new ApiResponse<bool> { Status = 404, Message = "Item not found." });
            }

            await _itemRepository.DeleteAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<bool>
            {
                Status = 200,
                Data = true,
                Message = "Master item deleted successfully.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // GET: api/work-orders/master-items/{id}
        [HttpGet("master-items/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<ItemDto>>> GetMasterItem(Guid id)
        {
            var item = await _itemRepository.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound(new ApiResponse<ItemDto> { Status = 404, Message = "Item not found." });
            }

            var itemDto = new Application.DTOs.ItemDto
            {
                Id = item.Id,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                Unit = item.Unit,
                UnitPrice = item.UnitPrice,
                PaymentType = item.PaymentType,
                ManagementArea = item.ManagementArea,
                Currency = item.Currency,
                IsActive = item.IsActive,
                ClientId = item.ClientId,
                ClientName = string.Empty, // Will be populated if needed
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                LastModifiedAt = item.LastModifiedAt,
                LastModifiedBy = item.LastModifiedBy
            };

            return Ok(new ApiResponse<ItemDto>
            {
                Status = 200,
                Data = itemDto,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // GET: api/work-orders/{id}/available-items
        [HttpGet("{id}/available-items")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ItemDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<ItemDto>>>> GetAvailableItemsForWorkOrder(Guid id)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(id);
            if (workOrder == null)
            {
                return NotFound(new ApiResponse<IEnumerable<ItemDto>>
                {
                    Status = 404,
                    Message = "Work order not found"
                });
            }

            if (!workOrder.ClientId.HasValue)
            {
                return BadRequest(new ApiResponse<IEnumerable<ItemDto>>
                {
                    Status = 400,
                    Message = "Work order must have a client to get available items"
                });
            }

            var availableItems = await _itemRepository.GetActiveItemsByClientIdAsync(workOrder.ClientId.Value);

            var itemDtos = availableItems.Select(item => new Application.DTOs.ItemDto
            {
                Id = item.Id,
                ItemNumber = item.ItemNumber,
                Description = item.Description,
                Unit = item.Unit,
                UnitPrice = item.UnitPrice,
                PaymentType = item.PaymentType,
                ManagementArea = item.ManagementArea,
                Currency = item.Currency,
                IsActive = item.IsActive,
                ClientId = item.ClientId,
                ClientName = item.Client?.Name ?? string.Empty,
                CreatedAt = item.CreatedAt,
                CreatedBy = item.CreatedBy,
                LastModifiedAt = item.LastModifiedAt,
                LastModifiedBy = item.LastModifiedBy
            }).ToList();

            return Ok(new ApiResponse<IEnumerable<Application.DTOs.ItemDto>>
            {
                Status = 200,
                Data = itemDtos,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // POST: api/work-orders/{id}/assign-item
        [HttpPost("{id}/assign-item")]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderItemAssignmentDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<WorkOrderItemAssignmentDto>>> AssignItemToWorkOrder(Guid id, [FromBody] AssignItemToWorkOrderDto request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<WorkOrderItemAssignmentDto>
                {
                    Status = 400,
                    Message = "Request data is required"
                });
            }

            var workOrder = await _workOrderRepository.GetByIdAsync(id);
            if (workOrder == null)
            {
                return NotFound(new ApiResponse<WorkOrderItemAssignmentDto>
                {
                    Status = 404,
                    Message = "Work order not found"
                });
            }

            var item = await _itemRepository.GetByIdAsync(request.ItemId);
            if (item == null)
            {
                return NotFound(new ApiResponse<WorkOrderItemAssignmentDto>
                {
                    Status = 404,
                    Message = "Item not found"
                });
            }

            // Check if item belongs to the same client as work order
            if (workOrder.ClientId != item.ClientId)
            {
                return BadRequest(new ApiResponse<WorkOrderItemAssignmentDto>
                {
                    Status = 400,
                    Message = "Item does not belong to the same client as the work order"
                });
            }

            // Check if item is already assigned to this work order
            var existingAssignment = await _workOrderItemAssignmentRepository.AssignmentExistsAsync(id, request.ItemId);
            if (existingAssignment)
            {
                return BadRequest(new ApiResponse<WorkOrderItemAssignmentDto>
                {
                    Status = 400,
                    Message = "Item is already assigned to this work order"
                });
            }

            var assignment = new WorkOrderItemAssignment
            {
                Id = Guid.NewGuid(),
                WorkOrderId = id,
                ItemId = request.ItemId,
                EstimatedQuantity = request.EstimatedQuantity,
                EstimatedPrice = item.UnitPrice * request.EstimatedQuantity,
                EstimatedPriceWithVAT = item.UnitPrice * request.EstimatedQuantity * 1.15m, // Assuming 15% VAT
                ActualQuantity = 0,
                ActualPrice = 0,
                ActualPriceWithVAT = 0,
                ReasonForFinalQuantity = request.ReasonForFinalQuantity,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = User?.Identity?.Name ?? "system",
                LastModifiedAt = DateTime.UtcNow,
                LastModifiedBy = User?.Identity?.Name ?? "system"
            };

            await _workOrderItemAssignmentRepository.AddAsync(assignment);
            await _unitOfWork.SaveChangesAsync();

            // Get the created assignment with item details
            var createdAssignment = await _workOrderItemAssignmentRepository.GetAssignmentByIdAsync(assignment.Id);

            var assignmentDto = new Application.DTOs.WorkOrderItemAssignmentDto
            {
                Id = createdAssignment.Id,
                WorkOrderId = createdAssignment.WorkOrderId,
                ItemId = createdAssignment.ItemId,
                EstimatedQuantity = createdAssignment.EstimatedQuantity,
                EstimatedPrice = createdAssignment.EstimatedPrice,
                EstimatedPriceWithVAT = createdAssignment.EstimatedPriceWithVAT,
                ActualQuantity = createdAssignment.ActualQuantity,
                ActualPrice = createdAssignment.ActualPrice,
                ActualPriceWithVAT = createdAssignment.ActualPriceWithVAT,
                ReasonForFinalQuantity = createdAssignment.ReasonForFinalQuantity,
                CreatedAt = createdAssignment.CreatedAt,
                CreatedBy = createdAssignment.CreatedBy,
                LastModifiedAt = createdAssignment.LastModifiedAt,
                LastModifiedBy = createdAssignment.LastModifiedBy,
                Item = createdAssignment.Item != null ? new Application.DTOs.ItemDto
                {
                    Id = createdAssignment.Item.Id,
                    ItemNumber = createdAssignment.Item.ItemNumber,
                    Description = createdAssignment.Item.Description,
                    Unit = createdAssignment.Item.Unit,
                    UnitPrice = createdAssignment.Item.UnitPrice,
                    PaymentType = createdAssignment.Item.PaymentType,
                    ManagementArea = createdAssignment.Item.ManagementArea,
                    Currency = createdAssignment.Item.Currency,
                    IsActive = createdAssignment.Item.IsActive,
                    ClientId = createdAssignment.Item.ClientId,
                    ClientName = createdAssignment.Item.Client?.Name ?? string.Empty,
                    CreatedAt = createdAssignment.Item.CreatedAt,
                    CreatedBy = createdAssignment.Item.CreatedBy,
                    LastModifiedAt = createdAssignment.Item.LastModifiedAt,
                    LastModifiedBy = createdAssignment.Item.LastModifiedBy
                } : null
            };

            return CreatedAtAction(nameof(GetWorkOrderItemAssignments), new { id }, new ApiResponse<Application.DTOs.WorkOrderItemAssignmentDto>
            {
                Status = 201,
                Data = assignmentDto,
                Message = "Item assigned to work order successfully",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // PUT: api/work-orders/{id}/item-assignments/{assignmentId}
        [HttpPut("{id}/item-assignments/{assignmentId}")]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderItemAssignmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<WorkOrderItemAssignmentDto>>> UpdateWorkOrderItemAssignment(Guid id, Guid assignmentId, [FromBody] UpdateWorkOrderItemAssignmentDto request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<WorkOrderItemAssignmentDto>
                {
                    Status = 400,
                    Message = "Request data is required"
                });
            }

            var assignment = await _workOrderItemAssignmentRepository.GetAssignmentByIdAsync(assignmentId);
            if (assignment == null || assignment.WorkOrderId != id)
            {
                return NotFound(new ApiResponse<WorkOrderItemAssignmentDto>
                {
                    Status = 404,
                    Message = "Item assignment not found"
                });
            }

            // Update fields if provided
            if (request.EstimatedQuantity.HasValue)
            {
                assignment.EstimatedQuantity = request.EstimatedQuantity.Value;
                assignment.EstimatedPrice = assignment.Item.UnitPrice * request.EstimatedQuantity.Value;
                assignment.EstimatedPriceWithVAT = assignment.Item.UnitPrice * request.EstimatedQuantity.Value * 1.15m;
            }

            if (request.ActualQuantity.HasValue)
            {
                assignment.ActualQuantity = request.ActualQuantity.Value;
                assignment.ActualPrice = assignment.Item.UnitPrice * request.ActualQuantity.Value;
                assignment.ActualPriceWithVAT = assignment.Item.UnitPrice * request.ActualQuantity.Value * 1.15m;
            }

            if (!string.IsNullOrEmpty(request.ReasonForFinalQuantity))
            {
                assignment.ReasonForFinalQuantity = request.ReasonForFinalQuantity;
            }

            assignment.LastModifiedAt = DateTime.UtcNow;
            assignment.LastModifiedBy = User?.Identity?.Name ?? "system";

            await _workOrderItemAssignmentRepository.UpdateAsync(assignment);
            await _unitOfWork.SaveChangesAsync();

            // Get updated assignment
            var updatedAssignment = await _workOrderItemAssignmentRepository.GetAssignmentByIdAsync(assignmentId);

            var assignmentDto = new Application.DTOs.WorkOrderItemAssignmentDto
            {
                Id = updatedAssignment.Id,
                WorkOrderId = updatedAssignment.WorkOrderId,
                ItemId = updatedAssignment.ItemId,
                EstimatedQuantity = updatedAssignment.EstimatedQuantity,
                EstimatedPrice = updatedAssignment.EstimatedPrice,
                EstimatedPriceWithVAT = updatedAssignment.EstimatedPriceWithVAT,
                ActualQuantity = updatedAssignment.ActualQuantity,
                ActualPrice = updatedAssignment.ActualPrice,
                ActualPriceWithVAT = updatedAssignment.ActualPriceWithVAT,
                ReasonForFinalQuantity = updatedAssignment.ReasonForFinalQuantity,
                CreatedAt = updatedAssignment.CreatedAt,
                CreatedBy = updatedAssignment.CreatedBy,
                LastModifiedAt = updatedAssignment.LastModifiedAt,
                LastModifiedBy = updatedAssignment.LastModifiedBy,
                Item = updatedAssignment.Item != null ? new Application.DTOs.ItemDto
                {
                    Id = updatedAssignment.Item.Id,
                    ItemNumber = updatedAssignment.Item.ItemNumber,
                    Description = updatedAssignment.Item.Description,
                    Unit = updatedAssignment.Item.Unit,
                    UnitPrice = updatedAssignment.Item.UnitPrice,
                    PaymentType = updatedAssignment.Item.PaymentType,
                    ManagementArea = updatedAssignment.Item.ManagementArea,
                    Currency = updatedAssignment.Item.Currency,
                    IsActive = updatedAssignment.Item.IsActive,
                    ClientId = updatedAssignment.Item.ClientId,
                    ClientName = updatedAssignment.Item.Client?.Name ?? string.Empty,
                    CreatedAt = updatedAssignment.Item.CreatedAt,
                    CreatedBy = updatedAssignment.Item.CreatedBy,
                    LastModifiedAt = updatedAssignment.Item.LastModifiedAt,
                    LastModifiedBy = updatedAssignment.Item.LastModifiedBy
                } : null
            };

            return Ok(new ApiResponse<Application.DTOs.WorkOrderItemAssignmentDto>
            {
                Status = 200,
                Data = assignmentDto,
                Message = "Item assignment updated successfully",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // DELETE: api/work-orders/{id}/item-assignments/{assignmentId}
        [HttpDelete("{id}/item-assignments/{assignmentId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveWorkOrderItemAssignment(Guid id, Guid assignmentId)
        {
            var assignment = await _workOrderItemAssignmentRepository.GetAssignmentByIdAsync(assignmentId);
            if (assignment == null || assignment.WorkOrderId != id)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Status = 404,
                    Message = "Item assignment not found"
                });
            }

            await _workOrderItemAssignmentRepository.RemoveAssignmentAsync(assignmentId);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<bool>
            {
                Status = 200,
                Data = true,
                Message = "Item assignment removed successfully",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // POST: api/WorkOrders/{id}/items
        [HttpPost("{id}/items")]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderItem>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<WorkOrderItem>>> AddWorkOrderItem(Guid id, [FromBody] WorkOrderItem item)
        {
            if (item == null)
            {
                return BadRequest(new ApiResponse<WorkOrderItem> { Status = 400, Message = "Item is required." });
            }
            item.WorkOrderId = id;
            item.Id = Guid.NewGuid();
            item.CreatedAt = DateTime.UtcNow;
            item.CreatedBy = User?.Identity?.Name ?? "system";
            await _workOrderRepository.AddItemAsync(item);
            await _unitOfWork.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWorkOrderItems), new { id }, new ApiResponse<WorkOrderItem>
            {
                Status = 201,
                Data = item,
                Message = "Item assigned to work order.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // PUT: api/WorkOrders/{id}/items
        [HttpPut("{id}/items")]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderDetailsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<WorkOrderDetailsDto>>> UpdateWorkOrderItem(Guid id, [FromBody] UpdateWorkOrderItemDto request)
        {
            if (request?.Item == null)
            {
                return BadRequest(new ApiResponse<WorkOrderDetailsDto> { Status = 400, Message = "Item data is required." });
            }

            if (!Guid.TryParse(request.Item.Id, out var itemId))
            {
                return BadRequest(new ApiResponse<WorkOrderDetailsDto> { Status = 400, Message = "Invalid item ID format." });
            }

            // Get the existing item
            var existingItem = await _workOrderRepository.GetItemByIdAsync(itemId);
            if (existingItem == null)
            {
                return NotFound(new ApiResponse<WorkOrderDetailsDto> { Status = 404, Message = "Work order item not found." });
            }

            // Verify the item belongs to the specified work order
            if (existingItem.WorkOrderId != id)
            {
                return BadRequest(new ApiResponse<WorkOrderDetailsDto> { Status = 400, Message = "Item does not belong to the specified work order." });
            }

            // Update the item properties
            existingItem.ItemNumber = request.Item.ItemNumber;
            existingItem.Description = request.Item.ShortDescription ?? request.Item.LongDescription;
            existingItem.Unit = request.Item.UOM;
            existingItem.UnitPrice = request.Item.UnitPrice;
            existingItem.EstimatedQuantity = request.Item.EstimatedQuantity;
            existingItem.PaymentType = request.Item.PaymentType;
            existingItem.ManagementArea = request.Item.ManagementArea;
            existingItem.Currency = request.Item.Currency;
            
            // Recalculate prices
            existingItem.EstimatedPrice = existingItem.UnitPrice * existingItem.EstimatedQuantity;
            existingItem.EstimatedPriceWithVAT = existingItem.EstimatedPrice * 1.15m; // Assuming 15% VAT
            
            existingItem.LastModifiedAt = DateTime.UtcNow;
            existingItem.LastModifiedBy = User?.Identity?.Name ?? "system";

            await _workOrderRepository.UpdateItemAsync(existingItem);
            await _unitOfWork.SaveChangesAsync();

            // Get the updated work order with all details
            var updatedWorkOrder = await _workOrderRepository.GetWorkOrderWithDetailsAsync(id);
            if (updatedWorkOrder == null)
            {
                return NotFound(new ApiResponse<WorkOrderDetailsDto> { Status = 404, Message = "Work order not found after update." });
            }

            // Convert to DTO
            var dto = new WorkOrderDetailsDto
            {
                Id = updatedWorkOrder.Id,
                WorkOrderNumber = updatedWorkOrder.WorkOrderNumber,
                InternalOrderNumber = updatedWorkOrder.InternalOrderNumber,
                Title = updatedWorkOrder.Title,
                Description = updatedWorkOrder.Description,
                Client = updatedWorkOrder.Client?.Name ?? string.Empty,
                Location = updatedWorkOrder.Location,
                Status = updatedWorkOrder.Status?.Name ?? string.Empty,
                Priority = updatedWorkOrder.Priority?.Name ?? string.Empty,
                Category = updatedWorkOrder.Category,
                Type = updatedWorkOrder.Type,
                Class = updatedWorkOrder.Class,
                CompletionPercentage = updatedWorkOrder.CompletionPercentage,
                ReceivedDate = updatedWorkOrder.ReceivedDate,
                StartDate = updatedWorkOrder.StartDate,
                DueDate = updatedWorkOrder.DueDate,
                TargetEndDate = updatedWorkOrder.TargetEndDate,
                CreatedDate = updatedWorkOrder.CreatedAt,
                CreatedBy = updatedWorkOrder.CreatedBy,
                LastUpdated = updatedWorkOrder.LastModifiedAt,
                EstimatedPrice = updatedWorkOrder.EstimatedCost ?? 0,
                EngineerInCharge = updatedWorkOrder.EngineerInCharge?.FullName ?? string.Empty,
                Permits = updatedWorkOrder.Permits?.Select(p => new PermitDto
                {
                    Type = p.Type,
                    Status = p.Status.ToString()
                }).ToList() ?? new List<PermitDto>()
            };

            return Ok(new ApiResponse<WorkOrderDetailsDto>
            {
                Status = 200,
                Data = dto,
                Message = "Work order item updated successfully.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // GET: api/work-orders/items
        [HttpGet("items")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorkOrderItem>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkOrderItem>>>> GetAllWorkOrderItems()
        {
            var items = await _workOrderRepository.GetAllItemsAsync();
            return Ok(new ApiResponse<IEnumerable<WorkOrderItem>>
            {
                Status = 200,
                Data = items,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // POST: api/work-orders/items
        [HttpPost("items")]
        [ProducesResponseType(typeof(ApiResponse<WorkOrderItem>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<WorkOrderItem>>> CreateWorkOrderItem([FromBody] CreateWorkOrderItemDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new ApiResponse<WorkOrderItem> { Status = 400, Message = "Item is required." });
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
                Status = 201,
                Data = item,
                Message = "Item created.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // POST: api/work-orders/{id}/materials
        [HttpPost("{id}/materials")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> AssignMaterial(Guid id, [FromBody] MaterialAssignmentDto request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<bool> { Status = 400, Message = "Material assignment data is required." });
            }

            // Get the work order to verify it exists
            var workOrder = await _workOrderRepository.GetByIdAsync(id);
            if (workOrder == null)
            {
                return NotFound(new ApiResponse<bool> { Status = 404, Message = "Work order not found." });
            }

            // Create material assignment
            var materialAssignment = new MaterialAssignment(
                materialType: request.MaterialType,
                assignDate: DateTime.UtcNow,
                assignedById: Guid.Parse(User?.Identity?.Name ?? "00000000-0000-0000-0000-000000000000"),
                quantity: request.Quantity,
                unit: request.Unit,
                storingLocation: request.StoringLocation,
                workOrderNumber: workOrder.WorkOrderNumber,
                workOrderId: id,
                purchasableMaterialId: request.PurchasableMaterialId,
                receivableMaterialId: request.ReceivableMaterialId,
                notes: request.Notes
            );

            await _workOrderRepository.AddMaterialAssignmentAsync(materialAssignment);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaterialAssignments), new { id }, new ApiResponse<bool>
            {
                Status = 201,
                Data = true,
                Message = "Material assigned successfully.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // GET: api/work-orders/{id}/materials
        [HttpGet("{id}/materials")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MaterialAssignment>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<IEnumerable<MaterialAssignment>>>> GetMaterialAssignments(Guid id)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(id);
            if (workOrder == null)
            {
                return NotFound(new ApiResponse<IEnumerable<MaterialAssignment>> { Status = 404, Message = "Work order not found." });
            }

            var materials = await _workOrderRepository.GetMaterialAssignmentsByWorkOrderIdAsync(id);
            return Ok(new ApiResponse<IEnumerable<MaterialAssignment>>
            {
                Status = 200,
                Data = materials,
                Message = "Success",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // PUT: api/work-orders/{id}/materials/{assignmentId}
        [HttpPut("{id}/materials/{assignmentId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateMaterialAssignment(Guid id, Guid assignmentId, [FromBody] UpdateMaterialAssignmentDto request)
        {
            if (request == null)
            {
                return BadRequest(new ApiResponse<bool> { Status = 400, Message = "Update data is required." });
            }

            var materialAssignment = await _workOrderRepository.GetMaterialAssignmentByIdAsync(assignmentId);
            if (materialAssignment == null)
            {
                return NotFound(new ApiResponse<bool> { Status = 404, Message = "Material assignment not found." });
            }

            if (materialAssignment.WorkOrderId != id)
            {
                return BadRequest(new ApiResponse<bool> { Status = 400, Message = "Material assignment does not belong to the specified work order." });
            }

            // Update the material assignment
            materialAssignment.UpdateDetails(
                quantity: request.Quantity,
                storingLocation: request.StoringLocation,
                notes: request.Notes
            );

            await _workOrderRepository.UpdateMaterialAssignmentAsync(materialAssignment);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<bool>
            {
                Status = 200,
                Data = true,
                Message = "Material assignment updated successfully.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // DELETE: api/work-orders/{id}/materials/{assignmentId}
        [HttpDelete("{id}/materials/{assignmentId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> RemoveMaterialAssignment(Guid id, Guid assignmentId)
        {
            var materialAssignment = await _workOrderRepository.GetMaterialAssignmentByIdAsync(assignmentId);
            if (materialAssignment == null)
            {
                return NotFound(new ApiResponse<bool> { Status = 404, Message = "Material assignment not found." });
            }

            if (materialAssignment.WorkOrderId != id)
            {
                return BadRequest(new ApiResponse<bool> { Status = 400, Message = "Material assignment does not belong to the specified work order." });
            }

            await _workOrderRepository.RemoveMaterialAssignmentAsync(assignmentId);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new ApiResponse<bool>
            {
                Status = 200,
                Data = true,
                Message = "Material assignment removed successfully.",
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            });
        }

        // GET: api/work-orders/dashboard-metrics
        [HttpGet("dashboard-metrics")]
        [ProducesResponseType(typeof(ApiResponse<DashboardMetricsDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<DashboardMetricsDto>>> GetDashboardMetrics()
        {
            try
            {
                var allWorkOrders = await _workOrderRepository.GetAllAsyncWithStatusPriorityClient();
                var now = DateTime.UtcNow;

                var metrics = new DashboardMetricsDto
                {
                    TotalWorkOrders = allWorkOrders.Count(),
                    CompletedWorkOrders = allWorkOrders.Count(wo => wo.Status?.Code == "COMPLETED"),
                    OngoingWorkOrders = allWorkOrders.Count(wo => wo.Status?.Code == "IN_PROGRESS"),
                    PendingWorkOrders = allWorkOrders.Count(wo => wo.Status?.Code == "PENDING"),
                    OverdueWorkOrders = allWorkOrders.Count(wo => 
                        wo.DueDate.HasValue && wo.DueDate.Value < now && 
                        wo.Status?.Code != "COMPLETED" && wo.Status?.Code != "CANCELLED"),
                    CancelledWorkOrders = allWorkOrders.Count(wo => wo.Status?.Code == "CANCELLED"),
                    WorkOrdersWithoutPO = allWorkOrders.Count(wo => string.IsNullOrEmpty(wo.PO)),
                    
                    // Financial metrics - using EstimatedCost as the basis
                    TotalExpectedAmount = allWorkOrders.Sum(wo => wo.EstimatedCost ?? 0),
                    TotalInvoicedAmount = allWorkOrders.Where(wo => wo.Status?.Code == "COMPLETED").Sum(wo => wo.EstimatedCost ?? 0),
                    TotalPartiallyInvoicedAmount = allWorkOrders.Where(wo => wo.Status?.Code == "IN_PROGRESS").Sum(wo => wo.EstimatedCost ?? 0) * 0.5m, // Assuming 50% invoiced for in-progress
                    TotalRemainingAmountToBeInvoiced = allWorkOrders.Where(wo => wo.Status?.Code != "COMPLETED").Sum(wo => wo.EstimatedCost ?? 0)
                };

                return Ok(new ApiResponse<DashboardMetricsDto>
                {
                    Status = 200,
                    Data = metrics,
                    Message = "Dashboard metrics retrieved successfully",
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<DashboardMetricsDto>
                {
                    Status = 500,
                    Message = $"Error retrieving dashboard metrics: {ex.Message}",
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
                });
            }
        }
    }
} 