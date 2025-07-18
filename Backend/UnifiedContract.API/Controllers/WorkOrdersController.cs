using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnifiedContract.API.Models;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Interfaces.Repositories;

namespace UnifiedContract.API.Controllers
{
    [Route("api/[controller]")]
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
        [ProducesResponseType(typeof(IEnumerable<WorkOrder>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetWorkOrders()
        {
            var workOrders = await _workOrderRepository.GetAllAsync();
            return Ok(workOrders);
        }

        // GET: api/WorkOrders/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(WorkOrder), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkOrder>> GetWorkOrder(Guid id)
        {
            var workOrder = await _workOrderRepository.GetWorkOrderWithDetailsAsync(id);

            if (workOrder == null)
            {
                return NotFound();
            }

            return Ok(workOrder);
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
    }
} 