using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;
using System;

namespace UnifiedContract.Domain.Events.HR
{
    public class EmployeeSkillAddedEvent : DomainEvent
    {
        public EmployeeSkill EmployeeSkill { get; }

        public EmployeeSkillAddedEvent(EmployeeSkill employeeSkill)
        {
            EmployeeSkill = employeeSkill;
        }
    }

    public class EmployeeSkillUpdatedEvent : DomainEvent
    {
        public EmployeeSkill EmployeeSkill { get; }
        public string PreviousProficiencyLevel { get; }
        public string PreviousNotes { get; }

        public EmployeeSkillUpdatedEvent(
            EmployeeSkill employeeSkill, 
            string previousProficiencyLevel, 
            string previousNotes)
        {
            EmployeeSkill = employeeSkill;
            PreviousProficiencyLevel = previousProficiencyLevel;
            PreviousNotes = previousNotes;
        }
    }

    public class EmployeeSkillProficiencyIncreasedEvent : DomainEvent
    {
        public EmployeeSkill EmployeeSkill { get; }

        public EmployeeSkillProficiencyIncreasedEvent(EmployeeSkill employeeSkill)
        {
            EmployeeSkill = employeeSkill;
        }
    }

    public class EmployeeSkillProficiencyDecreasedEvent : DomainEvent
    {
        public EmployeeSkill EmployeeSkill { get; }

        public EmployeeSkillProficiencyDecreasedEvent(EmployeeSkill employeeSkill)
        {
            EmployeeSkill = employeeSkill;
        }
    }

    public class EmployeeSkillCertifiedEvent : DomainEvent
    {
        public EmployeeSkill EmployeeSkill { get; }

        public EmployeeSkillCertifiedEvent(EmployeeSkill employeeSkill)
        {
            EmployeeSkill = employeeSkill;
        }
    }

    public class EmployeeSkillUncertifiedEvent : DomainEvent
    {
        public EmployeeSkill EmployeeSkill { get; }

        public EmployeeSkillUncertifiedEvent(EmployeeSkill employeeSkill)
        {
            EmployeeSkill = employeeSkill;
        }
    }

    public class EmployeeSkillLastUsedUpdatedEvent : DomainEvent
    {
        public EmployeeSkill EmployeeSkill { get; }

        public EmployeeSkillLastUsedUpdatedEvent(EmployeeSkill employeeSkill)
        {
            EmployeeSkill = employeeSkill;
        }
    }

    public class EmployeeSkillRemovedEvent : DomainEvent
    {
        public Guid EmployeeId { get; }
        public Guid SkillId { get; }

        public EmployeeSkillRemovedEvent(Guid employeeId, Guid skillId)
        {
            EmployeeId = employeeId;
            SkillId = skillId;
        }
    }
} 