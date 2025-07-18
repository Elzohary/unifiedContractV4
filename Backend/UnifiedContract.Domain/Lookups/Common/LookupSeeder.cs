using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UnifiedContract.Domain.Entities.Common;
using UnifiedContract.Domain.Entities.WorkOrder.Lookups;
using UnifiedContract.Domain.Entities.HR.Lookups;
using UnifiedContract.Domain.Entities.Resource.Lookups;

namespace UnifiedContract.Domain.Entities.Auth
{
    /// <summary>
    /// Helper class to seed lookup tables with initial values
    /// </summary>
    public static class LookupSeeder
    {
        public static void SeedAllLookups(ModelBuilder modelBuilder)
        {
            SeedWorkOrderStatuses(modelBuilder);
            SeedPriorityLevels(modelBuilder);
            SeedIssueStatuses(modelBuilder);
            SeedLeaveTypes(modelBuilder);
            SeedLeaveStatuses(modelBuilder);
            SeedEquipmentStatuses(modelBuilder);
            SeedMaintenanceTypes(modelBuilder);
            SeedMaintenanceStatuses(modelBuilder);
            SeedMaterialTypes(modelBuilder);
            SeedSupplierCategories(modelBuilder);
        }

        private static void SeedWorkOrderStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkOrderStatus>().HasData(
                CreateLookup<WorkOrderStatus>(1, "DRAFT", "Draft", "Work order is in draft state", false, true, false, "#808080"),
                CreateLookup<WorkOrderStatus>(2, "PENDING", "Pending", "Work order is pending approval", false, true, true, "#FFA500"),
                CreateLookup<WorkOrderStatus>(3, "APPROVED", "Approved", "Work order has been approved", false, true, false, "#008000"),
                CreateLookup<WorkOrderStatus>(4, "IN_PROGRESS", "In Progress", "Work order is in progress", false, true, false, "#0000FF"),
                CreateLookup<WorkOrderStatus>(5, "ON_HOLD", "On Hold", "Work order is temporarily on hold", false, true, false, "#FFD700"),
                CreateLookup<WorkOrderStatus>(6, "COMPLETED", "Completed", "Work order has been completed", true, false, false, "#006400"),
                CreateLookup<WorkOrderStatus>(7, "CANCELLED", "Cancelled", "Work order has been cancelled", true, false, false, "#FF0000")
            );
        }

        private static void SeedPriorityLevels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PriorityLevel>().HasData(
                CreateLookup<PriorityLevel>(1, "LOW", "Low", "Low priority", 1, "#008000", 48, false),
                CreateLookup<PriorityLevel>(2, "MEDIUM", "Medium", "Medium priority", 2, "#FFA500", 24, false),
                CreateLookup<PriorityLevel>(3, "HIGH", "High", "High priority", 3, "#FF0000", 8, false),
                CreateLookup<PriorityLevel>(4, "CRITICAL", "Critical", "Critical priority", 4, "#8B0000", 4, true)
            );
        }

        private static void SeedIssueStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IssueStatus>().HasData(
                CreateLookup<IssueStatus>(1, "OPEN", "Open", "Issue is open", false, true, "#FF0000"),
                CreateLookup<IssueStatus>(2, "IN_PROGRESS", "In Progress", "Issue is being addressed", false, true, "#FFA500"),
                CreateLookup<IssueStatus>(3, "RESOLVED", "Resolved", "Issue has been resolved", true, false, "#008000"),
                CreateLookup<IssueStatus>(4, "CLOSED", "Closed", "Issue has been closed", true, false, "#000080")
            );
        }

        private static void SeedLeaveTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaveType>().HasData(
                CreateLookup<LeaveType>(1, "ANNUAL", "Annual Leave", "Regular vacation time", 30, false, true, null, 7),
                CreateLookup<LeaveType>(2, "SICK", "Sick Leave", "Leave due to illness", 14, true, true, 14, 0),
                CreateLookup<LeaveType>(3, "MATERNITY", "Maternity Leave", "Leave for new mothers", 90, true, true, 90, 30),
                CreateLookup<LeaveType>(4, "PATERNITY", "Paternity Leave", "Leave for new fathers", 5, false, true, 5, 7),
                CreateLookup<LeaveType>(5, "BEREAVEMENT", "Bereavement", "Leave due to family death", 5, true, true, 5, 0),
                CreateLookup<LeaveType>(6, "UNPAID", "Unpaid Leave", "Leave without pay", 0, false, false, 30, 14),
                CreateLookup<LeaveType>(7, "STUDY", "Study Leave", "Leave for educational purposes", 10, true, true, 10, 14),
                CreateLookup<LeaveType>(8, "RELIGIOUS", "Religious Leave", "Leave for religious observances", 3, false, true, 3, 7),
                CreateLookup<LeaveType>(9, "SPECIAL", "Special Leave", "Leave for special circumstances", 0, true, true, null, 7)
            );
        }

        private static void SeedLeaveStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeaveStatus>().HasData(
                CreateLookup<LeaveStatus>(1, "PENDING", "Pending", "Leave request is pending approval", false, false, true, "#FFA500"),
                CreateLookup<LeaveStatus>(2, "APPROVED", "Approved", "Leave request has been approved", true, true, false, "#008000"),
                CreateLookup<LeaveStatus>(3, "REJECTED", "Rejected", "Leave request has been rejected", true, false, false, "#FF0000"),
                CreateLookup<LeaveStatus>(4, "CANCELLED", "Cancelled", "Leave request has been cancelled", true, false, false, "#808080")
            );
        }

        private static void SeedEquipmentStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EquipmentStatus>().HasData(
                CreateLookup<EquipmentStatus>(1, "AVAILABLE", "Available", "Equipment is available for use", true, false, true, "#008000"),
                CreateLookup<EquipmentStatus>(2, "IN_USE", "In Use", "Equipment is currently being used", false, true, true, "#0000FF"),
                CreateLookup<EquipmentStatus>(3, "UNDER_MAINTENANCE", "Under Maintenance", "Equipment is being serviced", false, true, false, "#FFA500"),
                CreateLookup<EquipmentStatus>(4, "DAMAGED", "Damaged", "Equipment is damaged", false, false, false, "#FF0000"),
                CreateLookup<EquipmentStatus>(5, "RETIRED", "Retired", "Equipment is no longer in service", false, false, false, "#808080")
            );
        }

        private static void SeedMaintenanceTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceType>().HasData(
                CreateLookup<MaintenanceType>(1, "ROUTINE", "Routine", "Regular scheduled maintenance", true, false, 4, 90, "#008000"),
                CreateLookup<MaintenanceType>(2, "REPAIR", "Repair", "Repair of damaged equipment", false, true, 8, null, "#FF0000"),
                CreateLookup<MaintenanceType>(3, "INSPECTION", "Inspection", "Safety or quality inspection", true, false, 2, 30, "#0000FF"),
                CreateLookup<MaintenanceType>(4, "EMERGENCY", "Emergency", "Urgent unplanned maintenance", false, true, 12, null, "#8B0000")
            );
        }

        private static void SeedMaintenanceStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaintenanceStatus>().HasData(
                CreateLookup<MaintenanceStatus>(1, "SCHEDULED", "Scheduled", "Maintenance is scheduled", false, true, "#FFA500"),
                CreateLookup<MaintenanceStatus>(2, "IN_PROGRESS", "In Progress", "Maintenance is in progress", false, true, "#0000FF"),
                CreateLookup<MaintenanceStatus>(3, "COMPLETED", "Completed", "Maintenance has been completed", true, false, "#008000"),
                CreateLookup<MaintenanceStatus>(4, "CANCELLED", "Cancelled", "Maintenance has been cancelled", true, false, "#FF0000")
            );
        }

        private static void SeedMaterialTypes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaterialType>().HasData(
                CreateLookup<MaterialType>(1, "PURCHASABLE", "Purchasable", "Material that needs to be purchased", true, false, true, true),
                CreateLookup<MaterialType>(2, "RECEIVABLE", "Receivable", "Material that is supplied by the client", false, true, true, false)
            );
        }

        private static void SeedSupplierCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierCategory>().HasData(
                CreateLookup<SupplierCategory>(1, "EQUIP", "Equipment", "Suppliers for equipment and tools", true, 30, true, "#0000FF"),
                CreateLookup<SupplierCategory>(2, "MATERIAL", "Materials", "Suppliers for construction materials", true, 45, false, "#8B4513"),
                CreateLookup<SupplierCategory>(3, "SERVICE", "Services", "Suppliers for contracted services", false, 15, true, "#008000"),
                CreateLookup<SupplierCategory>(4, "CONSULTANT", "Consultants", "Consulting firms and professionals", false, 30, true, "#800080"),
                CreateLookup<SupplierCategory>(5, "SUBCONTRACTOR", "Subcontractors", "Specialized subcontractors", true, 30, true, "#FF8C00")
            );
        }

        #region Helper Methods for Creating Lookup Entities

        private static T CreateLookup<T>(int id, string code, string name, string description, params object?[] specificProps) where T : Lookup, new()
        {
            var lookup = new T
            {
                Id = new Guid(id, 0, 0, new byte[8]),  // Convert int to Guid
                Code = code,
                Name = name,
                Description = description
            };

            if (typeof(T) == typeof(WorkOrderStatus) && specificProps.Length >= 3)
            {
                ((WorkOrderStatus)(object)lookup).IsCompleted = (bool)specificProps[0]!;
                ((WorkOrderStatus)(object)lookup).AllowsEditing = (bool)specificProps[1]!;
                ((WorkOrderStatus)(object)lookup).RequiresApproval = (bool)specificProps[2]!;
                if (specificProps.Length > 3 && specificProps[3] != null) 
                    ((WorkOrderStatus)(object)lookup).ColorCode = (string)specificProps[3]!;
            }
            else if (typeof(T) == typeof(PriorityLevel) && specificProps.Length >= 3)
            {
                ((PriorityLevel)(object)lookup).SeverityValue = (int)specificProps[0]!;
                ((PriorityLevel)(object)lookup).ColorCode = (string)specificProps[1]!;
                ((PriorityLevel)(object)lookup).TargetResponseHours = (int)specificProps[2]!;
                if (specificProps.Length > 3)
                    ((PriorityLevel)(object)lookup).RequiresImmediateNotification = (bool)specificProps[3]!;
            }
            else if (typeof(T) == typeof(IssueStatus) && specificProps.Length >= 2)
            {
                ((IssueStatus)(object)lookup).IsResolved = (bool)specificProps[0]!;
                ((IssueStatus)(object)lookup).AllowsEditing = (bool)specificProps[1]!;
                if (specificProps.Length > 2 && specificProps[2] != null)
                    ((IssueStatus)(object)lookup).ColorCode = (string)specificProps[2]!;
            }
            else if (typeof(T) == typeof(LeaveType) && specificProps.Length >= 5)
            {
                ((LeaveType)(object)lookup).DefaultDaysPerYear = (int)specificProps[0]!;
                ((LeaveType)(object)lookup).RequiresDocumentation = (bool)specificProps[1]!;
                ((LeaveType)(object)lookup).IsPaid = (bool)specificProps[2]!;
                if (specificProps[3] != null)
                    ((LeaveType)(object)lookup).MaxConsecutiveDays = (int)specificProps[3]!;
                if (specificProps[4] != null)
                    ((LeaveType)(object)lookup).MinimumNoticeDays = (int)specificProps[4]!;
            }
            else if (typeof(T) == typeof(LeaveStatus) && specificProps.Length >= 2)
            {
                ((LeaveStatus)(object)lookup).IsFinalState = (bool)specificProps[0]!;
                ((LeaveStatus)(object)lookup).DeductsFromBalance = (bool)specificProps[1]!;
                if (specificProps.Length > 2 && specificProps[2] != null)
                    ((LeaveStatus)(object)lookup).AllowsEditing = (bool)specificProps[2]!;
                if (specificProps.Length > 3 && specificProps[3] != null)
                    ((LeaveStatus)(object)lookup).ColorCode = (string)specificProps[3]!;
            }
            else if (typeof(T) == typeof(EquipmentStatus) && specificProps.Length >= 3)
            {
                var equipmentStatus = new EquipmentStatus(
                    code: code,
                    name: name,
                    description: description,
                    canBeAssigned: (bool)specificProps[0]!,
                    incursCost: (bool)specificProps[1]!,
                    isOperational: (bool)specificProps[2]!,
                    colorCode: specificProps.Length > 3 ? (string)specificProps[3]! : null
                );
                equipmentStatus.Id = new Guid(id, 0, 0, new byte[8]);  // Set the ID after creation
                lookup = (T)(object)equipmentStatus;
            }
            else if (typeof(T) == typeof(MaintenanceType) && specificProps.Length >= 1)
            {
                var maintenanceType = (MaintenanceType)(object)lookup;
                maintenanceType.IsScheduled = (bool)specificProps[0]!;
                if (specificProps.Length > 1 && specificProps[1] != null)
                    maintenanceType.RequiresOutOfService = (bool)specificProps[1]!;
                if (specificProps.Length > 2 && specificProps[2] != null)
                    maintenanceType.TypicalDurationHours = (int)specificProps[2]!;
                if (specificProps.Length > 3 && specificProps[3] != null)
                    maintenanceType.TypicalIntervalDays = (int)specificProps[3]!;
                if (specificProps.Length > 4 && specificProps[4] != null)
                    maintenanceType.ColorCode = (string)specificProps[4]!;
            }
            else if (typeof(T) == typeof(MaintenanceStatus) && specificProps.Length >= 2)
            {
                var maintenanceStatus = (MaintenanceStatus)(object)lookup;
                maintenanceStatus.IsFinalState = (bool)specificProps[0]!;
                maintenanceStatus.AllowsEditing = (bool)specificProps[1]!;
                if (specificProps.Length > 2 && specificProps[2] != null)
                    maintenanceStatus.ColorCode = (string)specificProps[2]!;
            }
            else if (typeof(T) == typeof(MaterialType) && specificProps.Length >= 4)
            {
                ((MaterialType)(object)lookup).IsPurchasable = (bool)specificProps[0]!;
                ((MaterialType)(object)lookup).IsReceivable = (bool)specificProps[1]!;
                ((MaterialType)(object)lookup).RequiresTracking = (bool)specificProps[2]!;
                ((MaterialType)(object)lookup).HasCost = (bool)specificProps[3]!;
            }
            else if (typeof(T) == typeof(SupplierCategory) && specificProps.Length >= 3)
            {
                var supplierCategory = new SupplierCategory(
                    code: code,
                    name: name,
                    description: description,
                    requiresVatNumber: (bool)specificProps[0]!,
                    defaultCreditDays: (decimal)specificProps[1]!,
                    requiresContractReview: (bool)specificProps[2]!,
                    colorCode: specificProps.Length > 3 ? (string)specificProps[3]! : null
                );
                supplierCategory.Id = new Guid(id, 0, 0, new byte[8]);  // Set the ID after creation
                lookup = (T)(object)supplierCategory;
            }

            return lookup;
        }

        #endregion
    }
} 