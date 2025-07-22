using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnifiedContract.API.Controllers;
using UnifiedContract.Domain.Entities.WorkOrder;
using UnifiedContract.Domain.Interfaces;
using UnifiedContract.Domain.Interfaces.Repositories;
using Xunit;

namespace UnifiedContract.Tests.Controllers
{
    public class WorkOrdersControllerTests
    {
        private readonly Mock<IWorkOrderRepository> _mockWorkOrderRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly WorkOrdersController _controller;

        public WorkOrdersControllerTests()
        {
            _mockWorkOrderRepository = new Mock<IWorkOrderRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new WorkOrdersController(_mockWorkOrderRepository.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetWorkOrders_ReturnsAllWorkOrders()
        {
            // Arrange
            var expectedWorkOrders = new List<WorkOrder>
            {
                new WorkOrder("WO-001", "Test Work Order 1", "Test Description 1", DateTime.Now, DateTime.Now.AddDays(7)),
                new WorkOrder("WO-002", "Test Work Order 2", "Test Description 2", DateTime.Now, DateTime.Now.AddDays(14))
            };

            _mockWorkOrderRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedWorkOrders);

            // Act
            var result = await _controller.GetWorkOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkOrders = Assert.IsAssignableFrom<IEnumerable<WorkOrder>>(okResult.Value);
            Assert.Equal(expectedWorkOrders.Count, returnedWorkOrders.Count());
        }

        [Fact]
        public async Task GetWorkOrder_WithValidId_ReturnsWorkOrder()
        {
            // Arrange
            var workOrderId = Guid.NewGuid();
            var expectedWorkOrder = new WorkOrder("WO-001", "Test Work Order", "Test Description", DateTime.Now, DateTime.Now.AddDays(7))
            {
                Id = workOrderId
            };

            _mockWorkOrderRepository.Setup(repo => repo.GetWorkOrderWithDetailsAsync(workOrderId))
                .ReturnsAsync(expectedWorkOrder);

            // Act
            var result = await _controller.GetWorkOrder(workOrderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkOrder = Assert.IsType<WorkOrder>(okResult.Value);
            Assert.Equal(expectedWorkOrder.Id, returnedWorkOrder.Id);
        }

        [Fact]
        public async Task GetWorkOrder_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var workOrderId = Guid.NewGuid();
            _mockWorkOrderRepository.Setup(repo => repo.GetWorkOrderWithDetailsAsync(workOrderId))
                .ReturnsAsync((WorkOrder)null);

            // Act
            var result = await _controller.GetWorkOrder(workOrderId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateWorkOrder_WithValidData_ReturnsCreatedWorkOrder()
        {
            // Arrange
            var workOrder = new WorkOrder("WO-001", "Test Work Order", "Test Description", DateTime.Now, DateTime.Now.AddDays(7));
            var createdWorkOrder = new WorkOrder("WO-001", "Test Work Order", "Test Description", DateTime.Now, DateTime.Now.AddDays(7))
            {
                Id = Guid.NewGuid()
            };

            _mockWorkOrderRepository.Setup(repo => repo.AddAsync(It.IsAny<WorkOrder>()))
                .ReturnsAsync(createdWorkOrder);

            // Act
            var result = await _controller.CreateWorkOrder(workOrder);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedWorkOrder = Assert.IsType<WorkOrder>(createdResult.Value);
            Assert.Equal(createdWorkOrder.Id, returnedWorkOrder.Id);
        }

        [Fact]
        public async Task UpdateWorkOrder_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var workOrderId = Guid.NewGuid();
            var workOrder = new WorkOrder("WO-001", "Updated Work Order", "Updated Description", DateTime.Now, DateTime.Now.AddDays(7))
            {
                Id = workOrderId
            };

            var existingWorkOrder = new WorkOrder("WO-001", "Original Work Order", "Original Description", DateTime.Now, DateTime.Now.AddDays(7))
            {
                Id = workOrderId
            };

            _mockWorkOrderRepository.Setup(repo => repo.GetByIdAsync(workOrderId))
                .ReturnsAsync(existingWorkOrder);

            // Act
            var result = await _controller.UpdateWorkOrder(workOrderId, workOrder);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockWorkOrderRepository.Verify(repo => repo.UpdateAsync(It.IsAny<WorkOrder>()), Times.Once);
        }

        [Fact]
        public async Task UpdateWorkOrder_WithInvalidId_ReturnsBadRequest()
        {
            // Arrange
            var workOrderId = Guid.NewGuid();
            var workOrder = new WorkOrder("WO-001", "Test Work Order", "Test Description", DateTime.Now, DateTime.Now.AddDays(7))
            {
                Id = Guid.NewGuid() // Different ID
            };

            // Act
            var result = await _controller.UpdateWorkOrder(workOrderId, workOrder);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateWorkOrder_WithNonExistentId_ReturnsNotFound()
        {
            // Arrange
            var workOrderId = Guid.NewGuid();
            var workOrder = new WorkOrder("WO-001", "Test Work Order", "Test Description", DateTime.Now, DateTime.Now.AddDays(7))
            {
                Id = workOrderId
            };

            _mockWorkOrderRepository.Setup(repo => repo.GetByIdAsync(workOrderId))
                .ReturnsAsync((WorkOrder)null);

            // Act
            var result = await _controller.UpdateWorkOrder(workOrderId, workOrder);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteWorkOrder_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var workOrderId = Guid.NewGuid();
            var workOrder = new WorkOrder("WO-001", "Test Work Order", "Test Description", DateTime.Now, DateTime.Now.AddDays(7))
            {
                Id = workOrderId
            };

            _mockWorkOrderRepository.Setup(repo => repo.GetByIdAsync(workOrderId))
                .ReturnsAsync(workOrder);

            // Act
            var result = await _controller.DeleteWorkOrder(workOrderId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockWorkOrderRepository.Verify(repo => repo.DeleteAsync(It.IsAny<WorkOrder>()), Times.Once);
        }

        [Fact]
        public async Task DeleteWorkOrder_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var workOrderId = Guid.NewGuid();
            _mockWorkOrderRepository.Setup(repo => repo.GetByIdAsync(workOrderId))
                .ReturnsAsync((WorkOrder)null);

            // Act
            var result = await _controller.DeleteWorkOrder(workOrderId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetWorkOrdersByClient_ReturnsWorkOrdersForClient()
        {
            // Arrange
            var clientName = "TestClient";
            var expectedWorkOrders = new List<WorkOrder>
            {
                new WorkOrder("WO-001", "Test Work Order 1", "Test Description 1", DateTime.Now, DateTime.Now.AddDays(7)),
                new WorkOrder("WO-002", "Test Work Order 2", "Test Description 2", DateTime.Now, DateTime.Now.AddDays(14))
            };

            _mockWorkOrderRepository.Setup(repo => repo.GetWorkOrdersByClientAsync(clientName))
                .ReturnsAsync(expectedWorkOrders);

            // Act
            var result = await _controller.GetWorkOrdersByClient(clientName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkOrders = Assert.IsAssignableFrom<IEnumerable<WorkOrder>>(okResult.Value);
            Assert.Equal(expectedWorkOrders.Count, returnedWorkOrders.Count());
        }

        [Fact]
        public async Task GetWorkOrdersByStatus_ReturnsWorkOrdersWithStatus()
        {
            // Arrange
            var status = "InProgress";
            var expectedWorkOrders = new List<WorkOrder>
            {
                new WorkOrder("WO-001", "Test Work Order 1", "Test Description 1", DateTime.Now, DateTime.Now.AddDays(7)),
                new WorkOrder("WO-002", "Test Work Order 2", "Test Description 2", DateTime.Now, DateTime.Now.AddDays(14))
            };

            _mockWorkOrderRepository.Setup(repo => repo.GetWorkOrdersWithStatusAsync(status))
                .ReturnsAsync(expectedWorkOrders);

            // Act
            var result = await _controller.GetWorkOrdersByStatus(status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedWorkOrders = Assert.IsAssignableFrom<IEnumerable<WorkOrder>>(okResult.Value);
            Assert.Equal(expectedWorkOrders.Count, returnedWorkOrders.Count());
        }
    }
} 