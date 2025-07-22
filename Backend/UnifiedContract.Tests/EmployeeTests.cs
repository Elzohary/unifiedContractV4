using System;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Domain.ValueObjects;
using Xunit;

namespace UnifiedContract.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var badgeNumber = "EMP001";
            var name = "John Doe";
            var jobTitle = "Engineer";
            var joinDate = DateTime.Today;
            var nationality = "Saudi";
            var address = new Address("123 Main St", "Riyadh", "Riyadh Province", "12345", "KSA");

            // Act
            var employee = new Employee(badgeNumber, name, jobTitle, joinDate, nationality, address);

            // Assert
            Assert.Equal(badgeNumber, employee.BadgeNumber);
            Assert.Equal(name, employee.Name);
            Assert.Equal(jobTitle, employee.JobTitle);
            Assert.Equal(joinDate, employee.JoinDate);
            Assert.Equal(nationality, employee.Nationality);
            Assert.Equal(address, employee.HomeAddress);
            Assert.Equal(100, employee.WorkTimeRatio);
            Assert.Equal(30, employee.OffDays);
        }

        [Fact]
        public void AssignDepartment_SetsDepartmentId()
        {
            // Arrange
            var employee = new Employee("EMP002", "Jane Smith", "Manager", DateTime.Today);
            var departmentId = Guid.NewGuid();

            // Act
            employee.AssignDepartment(departmentId);

            // Assert
            Assert.Equal(departmentId, employee.DepartmentId);
        }

        [Fact]
        public void AssignManager_SetsDirectManagerId()
        {
            // Arrange
            var employee = new Employee("EMP003", "Ali Ahmed", "Supervisor", DateTime.Today);
            var managerId = Guid.NewGuid();

            // Act
            employee.AssignManager(managerId);

            // Assert
            Assert.Equal(managerId, employee.DirectManagerId);
        }
    }
} 