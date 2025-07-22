using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UnifiedContract.API.Controllers;
using UnifiedContract.Application.DTOs.Resource;
using UnifiedContract.Application.Interfaces;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Interfaces;
using UnifiedContract.Domain.Interfaces.Repositories;
using Xunit;

namespace UnifiedContract.Tests.Controllers
{
    public class ClientMaterialsControllerTests
    {
        private readonly Mock<IClientMaterialRepository> _mockClientMaterialRepository;
        private readonly Mock<IRepository<ReceivableMaterial>> _mockReceivableMaterialRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ClientMaterialsController _controller;

        public ClientMaterialsControllerTests()
        {
            _mockClientMaterialRepository = new Mock<IClientMaterialRepository>();
            _mockReceivableMaterialRepository = new Mock<IRepository<ReceivableMaterial>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ClientMaterialsController(
                _mockClientMaterialRepository.Object,
                _mockReceivableMaterialRepository.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task GetClientMaterials_ReturnsAllClientMaterials()
        {
            // Arrange
            var clientMaterials = new List<ClientMaterial>
            {
                new ClientMaterial("GROUP1", 1, "MAT001", "Material 1", "PCS", Guid.NewGuid()),
                new ClientMaterial("GROUP2", 2, "MAT002", "Material 2", "KG", Guid.NewGuid())
            };

            var clientMaterialDtos = new List<ClientMaterialDto>
            {
                new ClientMaterialDto { Id = Guid.NewGuid(), MaterialMasterCode = "MAT001", Description = "Material 1" },
                new ClientMaterialDto { Id = Guid.NewGuid(), MaterialMasterCode = "MAT002", Description = "Material 2" }
            };

            _mockClientMaterialRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(clientMaterials);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ClientMaterialDto>>(clientMaterials))
                .Returns(clientMaterialDtos);

            // Act
            var result = await _controller.GetClientMaterials();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDtos = Assert.IsAssignableFrom<IEnumerable<ClientMaterialDto>>(okResult.Value);
            Assert.Equal(clientMaterialDtos.Count, returnedDtos.Count());
        }

        [Fact]
        public async Task GetClientMaterial_WithValidId_ReturnsClientMaterial()
        {
            // Arrange
            var clientMaterialId = Guid.NewGuid();
            var clientMaterial = new ClientMaterial("GROUP1", 1, "MAT001", "Material 1", "PCS", Guid.NewGuid())
            {
                Id = clientMaterialId
            };

            var clientMaterialDto = new ClientMaterialDto
            {
                Id = clientMaterialId,
                MaterialMasterCode = "MAT001",
                Description = "Material 1"
            };

            _mockClientMaterialRepository.Setup(repo => repo.GetByIdAsync(clientMaterialId))
                .ReturnsAsync(clientMaterial);
            _mockMapper.Setup(mapper => mapper.Map<ClientMaterialDto>(clientMaterial))
                .Returns(clientMaterialDto);

            // Act
            var result = await _controller.GetClientMaterial(clientMaterialId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDto = Assert.IsType<ClientMaterialDto>(okResult.Value);
            Assert.Equal(clientMaterialId, returnedDto.Id);
        }

        [Fact]
        public async Task GetClientMaterial_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var clientMaterialId = Guid.NewGuid();
            _mockClientMaterialRepository.Setup(repo => repo.GetByIdAsync(clientMaterialId))
                .ReturnsAsync((ClientMaterial)null);

            // Act
            var result = await _controller.GetClientMaterial(clientMaterialId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetClientMaterialsByClient_ReturnsClientMaterialsForClient()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var clientMaterials = new List<ClientMaterial>
            {
                new ClientMaterial("GROUP1", 1, "MAT001", "Material 1", "PCS", clientId),
                new ClientMaterial("GROUP2", 2, "MAT002", "Material 2", "KG", clientId)
            };

            var clientMaterialDtos = new List<ClientMaterialDto>
            {
                new ClientMaterialDto { Id = Guid.NewGuid(), MaterialMasterCode = "MAT001", Description = "Material 1" },
                new ClientMaterialDto { Id = Guid.NewGuid(), MaterialMasterCode = "MAT002", Description = "Material 2" }
            };

            _mockClientMaterialRepository.Setup(repo => repo.GetMaterialsByClientAsync(clientId))
                .ReturnsAsync(clientMaterials);
            _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ClientMaterialDto>>(clientMaterials))
                .Returns(clientMaterialDtos);

            // Act
            var result = await _controller.GetClientMaterialsByClient(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDtos = Assert.IsAssignableFrom<IEnumerable<ClientMaterialDto>>(okResult.Value);
            Assert.Equal(clientMaterialDtos.Count, returnedDtos.Count());
        }

        [Fact]
        public async Task CreateClientMaterial_WithValidData_ReturnsCreatedClientMaterial()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var createDto = new CreateClientMaterialDto
            {
                GroupCode = "GROUP1",
                SEQ = 1,
                MaterialMasterCode = "MAT001",
                Description = "Material 1",
                Unit = "PCS",
                ClientId = clientId
            };

            var clientMaterial = new ClientMaterial("GROUP1", 1, "MAT001", "Material 1", "PCS", clientId)
            {
                Id = Guid.NewGuid()
            };

            var clientMaterialDto = new ClientMaterialDto
            {
                Id = clientMaterial.Id,
                MaterialMasterCode = "MAT001",
                Description = "Material 1"
            };

            _mockClientMaterialRepository.Setup(repo => repo.IsMaterialCodeUniqueAsync("MAT001", clientId))
                .ReturnsAsync(true);
            _mockClientMaterialRepository.Setup(repo => repo.AddAsync(It.IsAny<ClientMaterial>()))
                .ReturnsAsync(clientMaterial);
            _mockMapper.Setup(mapper => mapper.Map<ClientMaterialDto>(clientMaterial))
                .Returns(clientMaterialDto);

            // Act
            var result = await _controller.CreateClientMaterial(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedDto = Assert.IsType<ClientMaterialDto>(createdResult.Value);
            Assert.Equal(clientMaterial.Id, returnedDto.Id);
        }

        [Fact]
        public async Task CreateClientMaterial_WithDuplicateMaterialCode_ReturnsBadRequest()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var createDto = new CreateClientMaterialDto
            {
                GroupCode = "GROUP1",
                SEQ = 1,
                MaterialMasterCode = "MAT001",
                Description = "Material 1",
                Unit = "PCS",
                ClientId = clientId
            };

            _mockClientMaterialRepository.Setup(repo => repo.IsMaterialCodeUniqueAsync("MAT001", clientId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.CreateClientMaterial(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("already exists", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task UpdateClientMaterial_WithValidData_ReturnsNoContent()
        {
            // Arrange
            var clientMaterialId = Guid.NewGuid();
            var updateDto = new UpdateClientMaterialDto
            {
                GroupCode = "GROUP1",
                SEQ = 1,
                MaterialMasterCode = "MAT001",
                Description = "Updated Material 1",
                Unit = "PCS"
            };

            var clientMaterial = new ClientMaterial("GROUP1", 1, "MAT001", "Original Material 1", "PCS", Guid.NewGuid())
            {
                Id = clientMaterialId
            };

            _mockClientMaterialRepository.Setup(repo => repo.GetByIdAsync(clientMaterialId))
                .ReturnsAsync(clientMaterial);

            // Act
            var result = await _controller.UpdateClientMaterial(clientMaterialId, updateDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockClientMaterialRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ClientMaterial>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateClientMaterial_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var clientMaterialId = Guid.NewGuid();
            var updateDto = new UpdateClientMaterialDto
            {
                GroupCode = "GROUP1",
                SEQ = 1,
                MaterialMasterCode = "MAT001",
                Description = "Updated Material 1",
                Unit = "PCS"
            };

            _mockClientMaterialRepository.Setup(repo => repo.GetByIdAsync(clientMaterialId))
                .ReturnsAsync((ClientMaterial)null);

            // Act
            var result = await _controller.UpdateClientMaterial(clientMaterialId, updateDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteClientMaterial_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var clientMaterialId = Guid.NewGuid();
            var clientMaterial = new ClientMaterial("GROUP1", 1, "MAT001", "Material 1", "PCS", Guid.NewGuid())
            {
                Id = clientMaterialId
            };

            var receivableMaterials = new List<ReceivableMaterial>(); // Empty list - no references

            _mockClientMaterialRepository.Setup(repo => repo.GetByIdAsync(clientMaterialId))
                .ReturnsAsync(clientMaterial);
            _mockReceivableMaterialRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(receivableMaterials);

            // Act
            var result = await _controller.DeleteClientMaterial(clientMaterialId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockClientMaterialRepository.Verify(repo => repo.DeleteAsync(It.IsAny<ClientMaterial>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteClientMaterial_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var clientMaterialId = Guid.NewGuid();
            _mockClientMaterialRepository.Setup(repo => repo.GetByIdAsync(clientMaterialId))
                .ReturnsAsync((ClientMaterial)null);

            // Act
            var result = await _controller.DeleteClientMaterial(clientMaterialId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteClientMaterial_WithReferencedReceivableMaterials_ReturnsBadRequest()
        {
            // Arrange
            var clientMaterialId = Guid.NewGuid();
            var clientMaterial = new ClientMaterial("GROUP1", 1, "MAT001", "Material 1", "PCS", Guid.NewGuid())
            {
                Id = clientMaterialId
            };

            var receivableMaterials = new List<ReceivableMaterial>
            {
                new ReceivableMaterial("MAT001", "Material 1", "PCS", 100, clientMaterialId)
            };

            _mockClientMaterialRepository.Setup(repo => repo.GetByIdAsync(clientMaterialId))
                .ReturnsAsync(clientMaterial);
            _mockReceivableMaterialRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(receivableMaterials);

            // Act
            var result = await _controller.DeleteClientMaterial(clientMaterialId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Contains("referenced", badRequestResult.Value.ToString());
        }
    }
} 