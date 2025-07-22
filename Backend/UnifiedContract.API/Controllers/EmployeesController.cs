using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnifiedContract.Domain.Entities.HR;
using UnifiedContract.Domain.Interfaces.Repositories;
using UnifiedContract.Domain.Interfaces;
using AutoMapper;
using UnifiedContract.Application.DTOs.HR;
using System.Linq;
using UnifiedContract.Domain.ValueObjects;

namespace UnifiedContract.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeesController(
            IRepository<Employee> employeeRepository,
            IRepository<Department> departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return Ok(employeeDtos);
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeDto>> GetEmployee(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }

        // GET: api/Employees/department/5
        [HttpGet("department/{departmentId}")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(Guid departmentId)
        {
            var employees = await _employeeRepository.GetAllAsync();
            var departmentEmployees = employees.Where(e => e.DepartmentId == departmentId);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(departmentEmployees);
            return Ok(employeeDtos);
        }

        // POST: api/Employees
        [HttpPost]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmployeeDto>> CreateEmployee([FromBody] CreateEmployeeDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate department exists
            var department = await _departmentRepository.GetByIdAsync(createDto.DepartmentId);
            if (department == null)
            {
                return BadRequest("Department not found");
            }

            // Check if employee badge number is unique
            var existingEmployees = await _employeeRepository.GetAllAsync();
            if (existingEmployees.Any(e => e.BadgeNumber == createDto.BadgeNumber))
            {
                return BadRequest($"Employee with badge number '{createDto.BadgeNumber}' already exists");
            }

            var employee = new Employee(
                createDto.BadgeNumber,
                createDto.Name,
                createDto.JobTitle,
                createDto.JoinDate,
                createDto.Nationality);

            // Assign department if provided
            if (createDto.DepartmentId != Guid.Empty)
            {
                employee.AssignDepartment(createDto.DepartmentId);
            }

            // Assign manager if provided
            if (createDto.DirectManagerId.HasValue && createDto.DirectManagerId.Value != Guid.Empty)
            {
                employee.AssignManager(createDto.DirectManagerId.Value);
            }

            // Update contact information if provided
            if (!string.IsNullOrEmpty(createDto.CompanyPhone) || !string.IsNullOrEmpty(createDto.PersonalPhone))
            {
                employee.UpdateContactInformation(createDto.CompanyPhone, createDto.PersonalPhone);
            }

            // Update salary if provided
            if (createDto.Salary > 0)
            {
                var salary = new Money(createDto.Salary, "SAR");
                employee.UpdateSalary(salary);
            }

            employee.CreatedBy = User.Identity?.Name ?? "system";
            var createdEmployee = await _employeeRepository.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            var employeeDto = _mapper.Map<EmployeeDto>(createdEmployee);
            return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.Id }, employeeDto);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return NotFound();
            }

            // Validate department exists if changed
            if (updateDto.DepartmentId.HasValue && updateDto.DepartmentId.Value != Guid.Empty)
            {
                var department = await _departmentRepository.GetByIdAsync(updateDto.DepartmentId.Value);
                if (department == null)
                {
                    return BadRequest("Department not found");
                }
                existingEmployee.AssignDepartment(updateDto.DepartmentId.Value);
            }

            // Check if employee badge number is unique if changed
            if (!string.IsNullOrEmpty(updateDto.BadgeNumber) && updateDto.BadgeNumber != existingEmployee.BadgeNumber)
            {
                var existingEmployees = await _employeeRepository.GetAllAsync();
                if (existingEmployees.Any(e => e.BadgeNumber == updateDto.BadgeNumber && e.Id != id))
                {
                    return BadRequest($"Employee with badge number '{updateDto.BadgeNumber}' already exists");
                }
            }

            // Update employee properties using domain methods
            if (!string.IsNullOrEmpty(updateDto.JobTitle))
                existingEmployee.UpdateJobInformation(updateDto.JobTitle);
            
            if (!string.IsNullOrEmpty(updateDto.CompanyPhone) || !string.IsNullOrEmpty(updateDto.PersonalPhone))
                existingEmployee.UpdateContactInformation(updateDto.CompanyPhone, updateDto.PersonalPhone);
            
            if (updateDto.Salary.HasValue && updateDto.Salary.Value > 0)
            {
                var salary = new Money(updateDto.Salary.Value, "SAR");
                existingEmployee.UpdateSalary(salary);
            }
            
            if (updateDto.DirectManagerId.HasValue && updateDto.DirectManagerId.Value != Guid.Empty)
                existingEmployee.AssignManager(updateDto.DirectManagerId.Value);

            existingEmployee.LastModifiedBy = User.Identity?.Name ?? "system";
            await _employeeRepository.UpdateAsync(existingEmployee);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            // Check if employee is a manager of other employees
            var allEmployees = await _employeeRepository.GetAllAsync();
            var hasSubordinates = allEmployees.Any(e => e.DirectManagerId == id);
            if (hasSubordinates)
            {
                return BadRequest("Cannot delete employee who is a manager of other employees");
            }

            await _employeeRepository.DeleteAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Employees/search?query=john
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> SearchEmployees([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query is required");
            }

            var employees = await _employeeRepository.GetAllAsync();
            var searchResults = employees.Where(e => 
                e.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                e.BadgeNumber.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                e.JobTitle.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                e.CompanyPhone.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                e.PersonalPhone.Contains(query, StringComparison.OrdinalIgnoreCase));

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(searchResults);
            return Ok(employeeDtos);
        }

        // GET: api/Employees/active
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetActiveEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var activeEmployees = employees.Where(e => e.IsActive);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(activeEmployees);
            return Ok(employeeDtos);
        }
    }
} 