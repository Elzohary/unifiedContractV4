using AutoMapper;
using UnifiedContract.Application.DTOs.Resource;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map ClientMaterial entity to DTOs
            CreateMap<ClientMaterial, ClientMaterialDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client != null ? src.Client.Name : null));
                
            // Add any other mappings your application needs
        }
    }
} 