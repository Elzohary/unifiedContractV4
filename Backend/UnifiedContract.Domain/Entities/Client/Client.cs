using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.ValueObjects;
using UnifiedContract.Domain.Entities.Auth;
namespace UnifiedContract.Domain.Entities.Client
{
    public class Client : BaseEntity
    {
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AlternatePhone { get; set; }
        public Address Address { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string VatNumber { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public Guid? AccountManagerId { get; set; }
        
        // Navigation properties
        public virtual User AccountManager { get; set; }
        public virtual ICollection<WorkOrder.WorkOrder> WorkOrders { get; set; }
        public virtual ICollection<ClientContact> Contacts { get; set; }
        
        public Client()
        {
            WorkOrders = new HashSet<WorkOrder.WorkOrder>();
            Contacts = new HashSet<ClientContact>();
        }
    }
} 