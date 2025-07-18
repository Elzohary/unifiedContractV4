using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Domain.Entities.Resource;
using UnifiedContract.Domain.Entities.WorkOrder.Lookups;
using UnifiedContract.Domain.Common;

namespace UnifiedContract.Domain.Entities.WorkOrder
{
    public class WorkOrder : BaseEntity
    {
        public string WorkOrderNumber { get; set; }
        public string InternalOrderNumber { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public string? Category { get; set; }
        public decimal CompletionPercentage { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? TargetEndDate { get; set; }
        public decimal? EstimatedCost { get; set; }
        
        // Foreign keys to lookup tables
        public Guid WorkOrderStatusId { get; set; }
        public Guid PriorityLevelId { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? EngineerInChargeId { get; set; }
        
        // Navigation properties
        public virtual WorkOrderStatus Status { get; set; }
        public virtual PriorityLevel Priority { get; set; }
        public virtual Client.Client Client { get; set; }
        public virtual Auth.User EngineerInCharge { get; set; }
        
        public virtual ICollection<WorkOrderItem> Items { get; set; }
        public virtual ICollection<WorkOrderRemark> Remarks { get; set; }
        public virtual ICollection<WorkOrderIssue> Issues { get; set; }
        public virtual ICollection<WorkOrderTask> Tasks { get; set; }
        public virtual ICollection<Permit> Permits { get; set; }
        public virtual ICollection<WorkOrderAction> Actions { get; set; }
        public virtual ICollection<ActionNeeded> ActionsNeeded { get; set; }
        public virtual ICollection<WorkOrderPhoto> Photos { get; set; }
        public virtual ICollection<WorkOrderForm> Forms { get; set; }
        public virtual ICollection<WorkOrderExpense> Expenses { get; set; }
        public virtual ICollection<WorkOrderInvoice> Invoices { get; set; }
        public virtual ICollection<MaterialAssignment> Materials { get; set; }
        public virtual ICollection<ManpowerAssignment> Manpower { get; set; }
        public virtual ICollection<EquipmentAssignment> Equipment { get; set; }
        
        // For expense breakdown
        public decimal MaterialsExpense { get; set; }
        public decimal LaborExpense { get; set; }
        public decimal OtherExpense { get; set; }
        
        public WorkOrder()
        {
            Items = new HashSet<WorkOrderItem>();
            Remarks = new HashSet<WorkOrderRemark>();
            Issues = new HashSet<WorkOrderIssue>();
            Tasks = new HashSet<WorkOrderTask>();
            Permits = new HashSet<Permit>();
            Actions = new HashSet<WorkOrderAction>();
            ActionsNeeded = new HashSet<ActionNeeded>();
            Photos = new HashSet<WorkOrderPhoto>();
            Forms = new HashSet<WorkOrderForm>();
            Expenses = new HashSet<WorkOrderExpense>();
            Invoices = new HashSet<WorkOrderInvoice>();
            Materials = new HashSet<MaterialAssignment>();
            Manpower = new HashSet<ManpowerAssignment>();
            Equipment = new HashSet<EquipmentAssignment>();
        }
    }
} 