using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Entities.HR;

namespace UnifiedContract.Domain.Events.HR
{
    public class SkillCreatedEvent : DomainEvent
    {
        public Skill Skill { get; }

        public SkillCreatedEvent(Skill skill)
        {
            Skill = skill;
        }
    }

    public class SkillUpdatedEvent : DomainEvent
    {
        public Skill Skill { get; }

        public SkillUpdatedEvent(Skill skill)
        {
            Skill = skill;
        }
    }

    public class SkillDeletedEvent : DomainEvent
    {
        public Skill Skill { get; }
        public string DeletionReason { get; }

        public SkillDeletedEvent(Skill skill, string deletionReason)
        {
            Skill = skill;
            DeletionReason = deletionReason;
        }
    }

    public class SkillActivatedEvent : DomainEvent
    {
        public Skill Skill { get; }
        public string ActivatedBy { get; }

        public SkillActivatedEvent(Skill skill, string activatedBy)
        {
            Skill = skill;
            ActivatedBy = activatedBy;
        }
    }

    public class SkillDeactivatedEvent : DomainEvent
    {
        public Skill Skill { get; }
        public string DeactivatedBy { get; }
        public string DeactivationReason { get; }

        public SkillDeactivatedEvent(Skill skill, string deactivatedBy, string deactivationReason)
        {
            Skill = skill;
            DeactivatedBy = deactivatedBy;
            DeactivationReason = deactivationReason;
        }
    }
} 