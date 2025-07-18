using System.Collections.Generic;
using UnifiedContract.Domain.Entities.Common;

namespace UnifiedContract.Domain.Entities.WorkOrder.Lookups
{
    /// <summary>
    /// Priority levels for work orders, tasks, issues, etc. (Low, Medium, High, Critical)
    /// </summary>
    public class PriorityLevel : Lookup
    {
        /// <summary>
        /// Numeric value representing the severity (e.g., 1-4)
        /// </summary>
        public int SeverityValue { get; set; }
        
        /// <summary>
        /// Color code for UI representation (hex or named color)
        /// </summary>
        public string ColorCode { get; set; }
        
        /// <summary>
        /// Target response time in hours
        /// </summary>
        public int? TargetResponseHours { get; set; }
        
        /// <summary>
        /// Whether this priority level requires immediate notification
        /// </summary>
        public bool RequiresImmediateNotification { get; set; }
        
        /// <summary>
        /// Navigation properties for related entities
        /// </summary>
        public virtual ICollection<WorkOrder> WorkOrders { get; set; }
        public virtual ICollection<WorkOrderTask> Tasks { get; set; }
        public virtual ICollection<WorkOrderIssue> Issues { get; set; }
        public virtual ICollection<WorkOrderAction> Actions { get; set; }
        public virtual ICollection<ActionNeeded> ActionsNeeded { get; set; }

        public PriorityLevel()
        {
            WorkOrders = new HashSet<WorkOrder>();
            Tasks = new HashSet<WorkOrderTask>();
            Issues = new HashSet<WorkOrderIssue>();
            Actions = new HashSet<WorkOrderAction>();
            ActionsNeeded = new HashSet<ActionNeeded>();
        }
    }
} 