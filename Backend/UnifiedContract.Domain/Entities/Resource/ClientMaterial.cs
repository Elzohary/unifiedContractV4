using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.Resource;

namespace UnifiedContract.Domain.Entities.Resource
{
    public class ClientMaterial : BaseEntity
    {
        public string GroupCode { get; private set; }
        public string SEQ { get; private set; }
        public string MaterialMasterCode { get; private set; }
        public string Description { get; private set; }
        public string Unit { get; private set; }
        public Guid ClientId { get; private set; }
        
        // Navigation property
        public virtual Client.Client Client { get; private set; }
        public virtual ICollection<ReceivableMaterial> ReceivableMaterials { get; private set; }
        
        // Required by EF Core
        private ClientMaterial() { }
        
        public ClientMaterial(
            string groupCode,
            string seq,
            string materialMasterCode,
            string description,
            string unit,
            Guid clientId)
        {
            GroupCode = groupCode;
            SEQ = seq;
            MaterialMasterCode = materialMasterCode;
            Description = description;
            Unit = unit;
            ClientId = clientId;
            ReceivableMaterials = new HashSet<ReceivableMaterial>();
            
            AddDomainEvent(new ClientMaterialCreatedEvent(this));
        }
        
        public void Update(
            string groupCode = null,
            string seq = null,
            string materialMasterCode = null,
            string description = null,
            string unit = null)
        {
            if (groupCode != null) GroupCode = groupCode;
            if (seq != null) SEQ = seq;
            if (materialMasterCode != null) MaterialMasterCode = materialMasterCode;
            if (description != null) Description = description;
            if (unit != null) Unit = unit;
        }
    }
} 