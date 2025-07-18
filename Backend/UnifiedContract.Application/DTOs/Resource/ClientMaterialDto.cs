using System;

namespace UnifiedContract.Application.DTOs.Resource
{
    public class ClientMaterialDto
    {
        public Guid Id { get; set; }
        public string GroupCode { get; set; }
        public string SEQ { get; set; }
        public string MaterialMasterCode { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
    }
    
    public class CreateClientMaterialDto
    {
        public string GroupCode { get; set; }
        public string SEQ { get; set; }
        public string MaterialMasterCode { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public Guid ClientId { get; set; }
    }
    
    public class UpdateClientMaterialDto
    {
        public string GroupCode { get; set; }
        public string SEQ { get; set; }
        public string MaterialMasterCode { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
    }
} 