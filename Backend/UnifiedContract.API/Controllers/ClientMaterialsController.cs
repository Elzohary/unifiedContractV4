using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnifiedContract.Application.DTOs.Resource;
using UnifiedContract.Application.Interfaces;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Interfaces.Repositories;
using UnifiedContract.Domain.Interfaces;
using System.Linq;

namespace UnifiedContract.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientMaterialsController : ControllerBase
    {
        private readonly IClientMaterialRepository _clientMaterialRepository;
        private readonly IRepository<ReceivableMaterial> _receivableMaterialRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientMaterialsController(
            IClientMaterialRepository clientMaterialRepository,
            IRepository<ReceivableMaterial> receivableMaterialRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _clientMaterialRepository = clientMaterialRepository;
            _receivableMaterialRepository = receivableMaterialRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/ClientMaterials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientMaterialDto>>> GetClientMaterials()
        {
            var clientMaterials = await _clientMaterialRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ClientMaterialDto>>(clientMaterials));
        }

        // GET: api/ClientMaterials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientMaterialDto>> GetClientMaterial(Guid id)
        {
            var clientMaterial = await _clientMaterialRepository.GetByIdAsync(id);

            if (clientMaterial == null)
            {
                return NotFound();
            }

            return _mapper.Map<ClientMaterialDto>(clientMaterial);
        }

        // GET: api/ClientMaterials/client/5
        [HttpGet("client/{clientId}")]
        public async Task<ActionResult<IEnumerable<ClientMaterialDto>>> GetClientMaterialsByClient(Guid clientId)
        {
            var clientMaterials = await _clientMaterialRepository.GetMaterialsByClientAsync(clientId);
            return Ok(_mapper.Map<IEnumerable<ClientMaterialDto>>(clientMaterials));
        }

        // POST: api/ClientMaterials
        [HttpPost]
        public async Task<ActionResult<ClientMaterialDto>> CreateClientMaterial(CreateClientMaterialDto createDto)
        {
            // Check if material code already exists for this client
            var isUnique = await _clientMaterialRepository.IsMaterialCodeUniqueAsync(createDto.MaterialMasterCode, createDto.ClientId);
            if (!isUnique)
            {
                return BadRequest($"Material with code '{createDto.MaterialMasterCode}' already exists for this client.");
            }

            var clientMaterial = new ClientMaterial(
                createDto.GroupCode,
                createDto.SEQ,
                createDto.MaterialMasterCode,
                createDto.Description,
                createDto.Unit,
                createDto.ClientId);

            await _clientMaterialRepository.AddAsync(clientMaterial);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetClientMaterial),
                new { id = clientMaterial.Id },
                _mapper.Map<ClientMaterialDto>(clientMaterial));
        }

        // PUT: api/ClientMaterials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClientMaterial(Guid id, UpdateClientMaterialDto updateDto)
        {
            var clientMaterial = await _clientMaterialRepository.GetByIdAsync(id);
            if (clientMaterial == null)
            {
                return NotFound();
            }

            clientMaterial.Update(
                updateDto.GroupCode,
                updateDto.SEQ,
                updateDto.MaterialMasterCode,
                updateDto.Description,
                updateDto.Unit);

            await _clientMaterialRepository.UpdateAsync(clientMaterial);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ClientMaterials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientMaterial(Guid id)
        {
            var clientMaterial = await _clientMaterialRepository.GetByIdAsync(id);
            if (clientMaterial == null)
            {
                return NotFound();
            }

            // Check if there are any receivable materials linked to this client material
            var receivableMaterials = (await _receivableMaterialRepository.GetAllAsync())
                .Where(rm => rm.ClientMaterialId == id);

            if (receivableMaterials.Any())
            {
                return BadRequest("Cannot delete this material as it is referenced by one or more receivable materials.");
            }

            await _clientMaterialRepository.DeleteAsync(clientMaterial);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
} 