using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.Resource;

namespace UnifiedContract.Domain.Events.Resource
{
    public class ClientMaterialCreatedEvent : DomainEvent
    {
        public ClientMaterial ClientMaterial { get; }
        
        public ClientMaterialCreatedEvent(ClientMaterial clientMaterial)
        {
            ClientMaterial = clientMaterial;
        }
    }
} 