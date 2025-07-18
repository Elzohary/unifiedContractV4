using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class EmployeeSkill : BaseEntity
    {
        private int _proficiencyLevel;
        private DateTime _acquiredDate;
        private string _certificate = string.Empty;
        private string _notes = string.Empty;
        private DateTime? _lastUsedDate;
        private bool _isFeatured;

        // Foreign keys
        public Guid EmployeeId { get; private set; }
        public Guid SkillId { get; private set; }

        // Navigation properties
        public Employee? Employee { get; private set; }
        public Skill? Skill { get; private set; }

        // Public properties with private setters
        public int ProficiencyLevel => _proficiencyLevel;
        public DateTime AcquiredDate => _acquiredDate;
        public string Certificate => _certificate;
        public string Notes => _notes;
        public DateTime? LastUsedDate => _lastUsedDate;
        public bool IsFeatured => _isFeatured;

        // Required by EF Core
        private EmployeeSkill() { }

        public EmployeeSkill(Guid employeeId, Guid skillId, int proficiencyLevel, DateTime acquiredDate, string? certificate = null, string? notes = null, DateTime? lastUsedDate = null, bool isFeatured = false)
        {
            ValidateEmployeeSkill(employeeId, skillId, proficiencyLevel);

            EmployeeId = employeeId;
            SkillId = skillId;
            _proficiencyLevel = proficiencyLevel;
            _acquiredDate = acquiredDate;
            _certificate = certificate ?? string.Empty;
            _notes = notes ?? string.Empty;
            _lastUsedDate = lastUsedDate;
            _isFeatured = isFeatured;

            AddDomainEvent(new EmployeeSkillAddedEvent(this));
        }

        public void UpdateDetails(int proficiencyLevel, DateTime acquiredDate, string? certificate = null, string? notes = null, DateTime? lastUsedDate = null)
        {
            ValidateProficiencyLevel(proficiencyLevel);

            string previousProficiencyLevel = _proficiencyLevel.ToString();
            string previousNotes = _notes;
            bool changed = false;

            if (_proficiencyLevel != proficiencyLevel)
            {
                _proficiencyLevel = proficiencyLevel;
                changed = true;
            }

            if (_acquiredDate != acquiredDate)
            {
                _acquiredDate = acquiredDate;
                changed = true;
            }

            if (_certificate != certificate)
            {
                _certificate = certificate ?? string.Empty;
                changed = true;
            }

            if (_notes != notes)
            {
                _notes = notes ?? string.Empty;
                changed = true;
            }

            if (_lastUsedDate != lastUsedDate && lastUsedDate.HasValue)
            {
                _lastUsedDate = lastUsedDate;
                changed = true;
            }

            if (changed)
            {
                AddDomainEvent(new EmployeeSkillUpdatedEvent(this, previousProficiencyLevel, previousNotes));
            }
        }

        public void SetFeatured(bool isFeatured)
        {
            if (_isFeatured != isFeatured)
            {
                _isFeatured = isFeatured;
                AddDomainEvent(new EmployeeSkillUpdatedEvent(this, _proficiencyLevel.ToString(), _notes));
            }
        }

        public void UpdateLastUsedDate(DateTime lastUsedDate)
        {
            if (!_lastUsedDate.HasValue || _lastUsedDate.Value < lastUsedDate)
            {
                _lastUsedDate = lastUsedDate;
                AddDomainEvent(new EmployeeSkillUpdatedEvent(this, _proficiencyLevel.ToString(), _notes));
            }
        }

        private void ValidateEmployeeSkill(Guid employeeId, Guid skillId, int proficiencyLevel)
        {
            if (employeeId == Guid.Empty)
                throw new ArgumentException("Employee ID is required.", nameof(employeeId));

            if (skillId == Guid.Empty)
                throw new ArgumentException("Skill ID is required.", nameof(skillId));

            ValidateProficiencyLevel(proficiencyLevel);
        }

        private void ValidateProficiencyLevel(int proficiencyLevel)
        {
            if (proficiencyLevel < 1 || proficiencyLevel > 5)
                throw new ArgumentOutOfRangeException(nameof(proficiencyLevel), "Proficiency level must be between 1 and 5.");
        }
    }
} 