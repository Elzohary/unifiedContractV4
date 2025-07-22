using System;
using System.Linq;
using UnifiedContract.Domain.Entities.WorkOrder;
using Xunit;

namespace UnifiedContract.Tests
{
    public class WorkOrderTests
    {
        [Fact]
        public void Constructor_InitializesCollections()
        {
            // Act
            var workOrder = new WorkOrder();

            // Assert
            Assert.NotNull(workOrder.Items);
            Assert.NotNull(workOrder.Remarks);
            Assert.NotNull(workOrder.Issues);
            Assert.NotNull(workOrder.Tasks);
            Assert.NotNull(workOrder.Permits);
            Assert.NotNull(workOrder.Actions);
            Assert.NotNull(workOrder.ActionsNeeded);
            Assert.NotNull(workOrder.Photos);
            Assert.NotNull(workOrder.Forms);
            Assert.NotNull(workOrder.Expenses);
            Assert.NotNull(workOrder.Invoices);
            Assert.NotNull(workOrder.Materials);
            Assert.NotNull(workOrder.Manpower);
            Assert.NotNull(workOrder.Equipment);
            Assert.NotNull(workOrder.SiteReports);
        }

        [Fact]
        public void CanSetAndGetProperties()
        {
            // Arrange
            var workOrder = new WorkOrder
            {
                WorkOrderNumber = "WO-001",
                InternalOrderNumber = "INT-001",
                Title = "Test Work Order",
                Description = "Test Description",
                Location = "Test Location",
                Category = "General",
                CompletionPercentage = 50.5m,
                ReceivedDate = DateTime.Today,
                StartDate = DateTime.Today.AddDays(1),
                DueDate = DateTime.Today.AddDays(10),
                TargetEndDate = DateTime.Today.AddDays(15),
                EstimatedCost = 1000.0m,
                WorkOrderStatusId = Guid.NewGuid(),
                PriorityLevelId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                EngineerInChargeId = Guid.NewGuid(),
                MaterialsExpense = 100,
                LaborExpense = 200,
                OtherExpense = 50
            };

            // Assert
            Assert.Equal("WO-001", workOrder.WorkOrderNumber);
            Assert.Equal("INT-001", workOrder.InternalOrderNumber);
            Assert.Equal("Test Work Order", workOrder.Title);
            Assert.Equal("Test Description", workOrder.Description);
            Assert.Equal("Test Location", workOrder.Location);
            Assert.Equal("General", workOrder.Category);
            Assert.Equal(50.5m, workOrder.CompletionPercentage);
            Assert.Equal(DateTime.Today, workOrder.ReceivedDate);
            Assert.Equal(DateTime.Today.AddDays(1), workOrder.StartDate);
            Assert.Equal(DateTime.Today.AddDays(10), workOrder.DueDate);
            Assert.Equal(DateTime.Today.AddDays(15), workOrder.TargetEndDate);
            Assert.Equal(1000.0m, workOrder.EstimatedCost);
            Assert.NotEqual(Guid.Empty, workOrder.WorkOrderStatusId);
            Assert.NotEqual(Guid.Empty, workOrder.PriorityLevelId);
            Assert.NotEqual(Guid.Empty, workOrder.ClientId);
            Assert.NotEqual(Guid.Empty, workOrder.EngineerInChargeId);
            Assert.Equal(100, workOrder.MaterialsExpense);
            Assert.Equal(200, workOrder.LaborExpense);
            Assert.Equal(50, workOrder.OtherExpense);
        }
    }
} 