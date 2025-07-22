using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnifiedContract.API.Controllers;
using UnifiedContract.Application.DTOs.HR;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Domain.Interfaces;
using UnifiedContract.Domain.Interfaces.Repositories;
using Xunit;

namespace UnifiedContract.Tests.Controllers
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IRepository<Employee>> _mockEmployeeRepository;
        private readonly Mock<IRepository<Department>> _mockDepartmentRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly EmployeesController _controller;

        public EmployeesControllerTests()
        {
            _mockEmployeeRepository = new Mock<IRepository<Employee>>();
            _mockDepartmentRepository = new Mock<IRepository<Department>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _controller = new EmployeesController(
                _mockEmployeeRepository.Object,
                _mockDepartmentRepository.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task GetEmployees_ReturnsAllEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee("EMP001", "John", "Doe", "john@test.com", "123456789", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-2), Guid.NewGuid(), "Developer", 50000, null),
                new Employee("EMP002", "Jane", "Smith", "jane@test.com", "987654321", DateTime.Now.AddYears(-25), DateTime.Now.AddYears(-1), Guid.NewGuid(), "Manager", 60000, null)
            };

            var employeeDtos = new List<EmployeeDto>
            {
                new EmployeeDto { Id = Guid.NewGuid(), EmployeeNumber = "EMP001", FirstName = "John", LastName = "Doe" },
                new EmployeeDto { Id = Guid.NewGuid(), EmployeeNumber = "EMP002", FirstName = "Jane", LastName = "Smith" }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(employees);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<EmployeeDto>>(employees))
                .Returns(employeeDtos);

            // Act
            var result = await _controller.GetEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDtos = Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(okResult.Value);
            Assert.Equal(employeeDtos.Count, returnedDtos.Count());
        }

        [Fact]
        public async Task GetEmployee_WithValidId_ReturnsEmployee()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new Employee("EMP001", "John", "Doe", "john@test.com", "123456789", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-2), Guid.NewGuid(), "Developer", 50000, null)
            {
                Id = employeeId
            };

            var employeeDto = new EmployeeDto
            {
                Id = employeeId,
                EmployeeNumber = "EMP001",
                FirstName = "John",
                LastName = "Doe"
            };

            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _mockMapper.Setup(mapper => mapper.Map<EmployeeDto>(employee))
                .Returns(employeeDto);

            // Act
            var result = await _controller.GetEmployee(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDto = Assert.IsType<EmployeeDto>(okResult.Value);
            Assert.Equal(employeeId, returnedDto.Id);
        }

        [Fact]
        public async Task GetEmployee_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.GetEmployee(employeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateEmployee_WithValidData_ReturnsCreatedEmployee()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var createDto = new CreateEmployeeDto
            {
                EmployeeNumber = "EMP001",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "123456789",
                DateOfBirth = DateTime.Now.AddYears(-30),
                HireDate = DateTime.Now.AddYears(-2),
                DepartmentId = departmentId,
                Position = "Developer",
                Salary = 50000
            };

            var department = new Department("IT", "Information Technology", "IT Department");
            var employee = new Employee("EMP001", "John", "Doe", "john@test.com", "123456789", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-2), departmentId, "Developer", 50000, null)
            {
                Id = Guid.NewGuid()
            };

            var employeeDto = new EmployeeDto
            {
                Id = employee.Id,
                EmployeeNumber = "EMP001",
                FirstName = "John",
                LastName = "Doe"
            };

            var existingEmployees = new List<Employee>(); // Empty list - no duplicates

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync(department);
            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingEmployees);
            _mockEmployeeRepository.Setup(repo => repo.AddAsync(It.IsAny<Employee>()))
                .ReturnsAsync(employee);
            _mockMapper.Setup(mapper => mapper.Map<EmployeeDto>(employee))
                .Returns(employeeDto);

            // Act
            var result = await _controller.CreateEmployee(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedDto = Assert.IsType<EmployeeDto>(createdResult.Value);
            Assert.Equal(employee.Id, returnedDto.Id);
        }

        [Fact]
        public async Task CreateEmployee_WithNonExistentDepartment_ReturnsBadRequest()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var createDto = new CreateEmployeeDto
            {
                EmployeeNumber = "EMP001",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "123456789",
                DateOfBirth = DateTime.Now.AddYears(-30),
                HireDate = DateTime.Now.AddYears(-2),
                DepartmentId = departmentId,
                Position = "Developer",
                Salary = 50000
            };

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync((Department)null);

            // Act
            var result = await _controller.CreateEmployee(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Department not found", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task CreateEmployee_WithDuplicateEmployeeNumber_ReturnsBadRequest()
        {
            // Arrange
            var departmentId = Guid.NewGuid();
            var createDto = new CreateEmployeeDto
            {
                EmployeeNumber = "EMP001",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                PhoneNumber = "123456789",
                DateOfBirth = DateTime.Now.AddYears(-30),
                HireDate = DateTime.Now.AddYears(-2),
                DepartmentId = departmentId,
                Position = "Developer",
                Salary = 50000
            };

            var department = new Department("IT", "Information Technology", "IT Department");
            var existingEmployees = new List<Employee>
            {
                new Employee("EMP001", "Jane", "Smith", "jane@test.com", "987654321", DateTime.Now.AddYears(-25), DateTime.Now.AddYears(-1), departmentId, "Manager", 60000, null)
            };

            _mockDepartmentRepository.Setup(repo => repo.GetByIdAsync(departmentId))
                .ReturnsAsync(department);
            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingEmployees);

            // Act
            var result = await _controller.CreateEmployee(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("already exists", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateEmployee_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var updateDto = new UpdateEmployeeDto
            {
                FirstName = "Updated John",
                LastName = "Updated Doe",
                Email = "updated.john@test.com",
                Salary = 55000
            };

            var existingEmployee = new Employee("EMP001", "John", "Doe", "john@test.com", "123456789", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-2), Guid.NewGuid(), "Developer", 50000, null)
            {
                Id = employeeId
            };

            var existingEmployees = new List<Employee> { existingEmployee };

            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
                .ReturnsAsync(existingEmployee);
            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(existingEmployees);

            // Act
            var result = await _controller.UpdateEmployee(employeeId, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockEmployeeRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Employee>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployee_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var updateDto = new UpdateEmployeeDto
            {
                FirstName = "Updated John"
            };

            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.UpdateEmployee(employeeId, updateDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new Employee("EMP001", "John", "Doe", "john@test.com", "123456789", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-2), Guid.NewGuid(), "Developer", 50000, null)
            {
                Id = employeeId
            };

            var allEmployees = new List<Employee> { employee }; // No subordinates

            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(allEmployees);

            // Act
            var result = await _controller.DeleteEmployee(employeeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockEmployeeRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Employee>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act
            var result = await _controller.DeleteEmployee(employeeId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteEmployee_WithSubordinates_ReturnsBadRequest()
        {
            // Arrange
            var managerId = Guid.NewGuid();
            var manager = new Employee("EMP001", "John", "Doe", "john@test.com", "123456789", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-2), Guid.NewGuid(), "Manager", 60000, null)
            {
                Id = managerId
            };

            var subordinate = new Employee("EMP002", "Jane", "Smith", "jane@test.com", "987654321", DateTime.Now.AddYears(-25), DateTime.Now.AddYears(-1), Guid.NewGuid(), "Developer", 50000, managerId);

            var allEmployees = new List<Employee> { manager, subordinate };

            _mockEmployeeRepository.Setup(repo => repo.GetByIdAsync(managerId))
                .ReturnsAsync(manager);
            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(allEmployees);

            // Act
            var result = await _controller.DeleteEmployee(managerId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("manager of other employees", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task SearchEmployees_WithValidQuery_ReturnsMatchingEmployees()
        {
            // Arrange
            var query = "john";
            var employees = new List<Employee>
            {
                new Employee("EMP001", "John", "Doe", "john@test.com", "123456789", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-2), Guid.NewGuid(), "Developer", 50000, null),
                new Employee("EMP002", "Jane", "Smith", "jane@test.com", "987654321", DateTime.Now.AddYears(-25), DateTime.Now.AddYears(-1), Guid.NewGuid(), "Manager", 60000, null)
            };

            var employeeDtos = new List<EmployeeDto>
            {
                new EmployeeDto { Id = Guid.NewGuid(), EmployeeNumber = "EMP001", FirstName = "John", LastName = "Doe" }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(employees);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<EmployeeDto>>(It.IsAny<IEnumerable<Employee>>()))
                .Returns(employeeDtos);

            // Act
            var result = await _controller.SearchEmployees(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDtos = Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(okResult.Value);
            Assert.Single(returnedDtos);
        }

        [Fact]
        public async Task SearchEmployees_WithEmptyQuery_ReturnsBadRequest()
        {
            // Arrange
            var query = "";

            // Act
            var result = await _controller.SearchEmployees(query);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("Search query is required", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task GetActiveEmployees_ReturnsOnlyActiveEmployees()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee("EMP001", "John", "Doe", "john@test.com", "123456789", DateTime.Now.AddYears(-30), DateTime.Now.AddYears(-2), Guid.NewGuid(), "Developer", 50000, null) { IsActive = true },
                new Employee("EMP002", "Jane", "Smith", "jane@test.com", "987654321", DateTime.Now.AddYears(-25), DateTime.Now.AddYears(-1), Guid.NewGuid(), "Manager", 60000, null) { IsActive = false }
            };

            var employeeDtos = new List<EmployeeDto>
            {
                new EmployeeDto { Id = Guid.NewGuid(), EmployeeNumber = "EMP001", FirstName = "John", LastName = "Doe", IsActive = true }
            };

            _mockEmployeeRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(employees);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<EmployeeDto>>(It.IsAny<IEnumerable<Employee>>()))
                .Returns(employeeDtos);

            // Act
            var result = await _controller.GetActiveEmployees();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDtos = Assert.IsAssignableFrom<IEnumerable<EmployeeDto>>(okResult.Value);
            Assert.Single(returnedDtos);
        }
    }
} 