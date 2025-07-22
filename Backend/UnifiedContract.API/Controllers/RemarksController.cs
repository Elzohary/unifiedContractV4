using Microsoft.AspNetCore.Mvc;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Interfaces.Repositories;

namespace UnifiedContract.API.Controllers
{
    [ApiController]
    [Route("api/remarks")]
    public class RemarksController : ControllerBase
    {
        private readonly IRepository<WorkOrderRemark> _remarkRepository;

        public RemarksController(IRepository<WorkOrderRemark> remarkRepository)
        {
            _remarkRepository = remarkRepository;
        }

        [HttpGet("work-order/{workOrderId}")]
        public async Task<IActionResult> GetRemarksByWorkOrder(Guid workOrderId)
        {
            var remarks = await _remarkRepository.GetAllAsync();
            var filteredRemarks = remarks.Where(r => r.WorkOrderId == workOrderId);
            return Ok(filteredRemarks);
        }
    }
} 