using System;
using System.Collections.Generic;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Education : BaseEntity
    {
        private string _institution = string.Empty;
        private string _degree = string.Empty;
        private string _fieldOfStudy = string.Empty;
        private DateTime _startDate;
        private DateTime? _endDate;
        private string _description = string.Empty;
        private string _documentUrl = string.Empty;
        private bool _isVerified;
        private Guid _employeeId;

        // Navigation property
        public Employee? Employee { get; private set; }

        // Public properties with private setters
        public string Institution => _institution;
        public string Degree => _degree;
        public string FieldOfStudy => _fieldOfStudy;
        public DateTime StartDate => _startDate;
        public DateTime? EndDate => _endDate;
        public string Description => _description;
        public string DocumentUrl => _documentUrl;
        public bool IsVerified => _isVerified;
        public Guid EmployeeId => _employeeId;

        // Required by EF Core
        private Education() { }

        public Education(
            Guid employeeId,
            string institution, 
            string degree, 
            string fieldOfStudy, 
            DateTime startDate, 
            DateTime? endDate = null,
            string? description = null,
            string? documentUrl = null)
        {
            ValidateRequiredFields(institution, degree, fieldOfStudy);
            ValidateDates(startDate, endDate);

            _employeeId = employeeId;
            _institution = institution;
            _degree = degree;
            _fieldOfStudy = fieldOfStudy;
            _startDate = startDate;
            _endDate = endDate;
            _description = description ?? string.Empty;
            _documentUrl = documentUrl ?? string.Empty;
            _isVerified = false;

            AddDomainEvent(new EducationAddedEvent(this));
        }

        public void UpdateDetails(
            string institution, 
            string degree, 
            string fieldOfStudy, 
            DateTime startDate, 
            DateTime? endDate = null,
            string? description = null,
            string? documentUrl = null)
        {
            ValidateRequiredFields(institution, degree, fieldOfStudy);
            ValidateDates(startDate, endDate);

            _institution = institution;
            _degree = degree;
            _fieldOfStudy = fieldOfStudy;
            _startDate = startDate;
            _endDate = endDate;
            _description = description ?? string.Empty;
            _documentUrl = documentUrl ?? string.Empty;

            AddDomainEvent(new EducationUpdatedEvent(this));
        }

        public void VerifyEducation()
        {
            if (_isVerified)
                throw new InvalidOperationException("Education record is already verified.");

            _isVerified = true;
            AddDomainEvent(new EducationVerifiedEvent(this));
        }

        public void RejectVerification()
        {
            if (!_isVerified)
                throw new InvalidOperationException("Education record is not verified.");

            _isVerified = false;
            AddDomainEvent(new EducationRejectedEvent(this));
        }

        public void UpdateDocumentUrl(string documentUrl)
        {
            if (string.IsNullOrWhiteSpace(documentUrl))
                throw new ArgumentException("Document URL cannot be empty.", nameof(documentUrl));

            _documentUrl = documentUrl;
            AddDomainEvent(new EducationUpdatedEvent(this));
        }

        public bool IsComplete()
        {
            return _endDate.HasValue;
        }

        private void ValidateRequiredFields(string institution, string degree, string fieldOfStudy)
        {
            if (string.IsNullOrWhiteSpace(institution))
                throw new ArgumentException("Institution is required.", nameof(institution));

            if (string.IsNullOrWhiteSpace(degree))
                throw new ArgumentException("Degree is required.", nameof(degree));

            if (string.IsNullOrWhiteSpace(fieldOfStudy))
                throw new ArgumentException("Field of study is required.", nameof(fieldOfStudy));
        }

        private void ValidateDates(DateTime startDate, DateTime? endDate)
        {
            if (startDate == default)
                throw new ArgumentException("Start date cannot be the default value.", nameof(startDate));

            if (endDate.HasValue && endDate.Value < startDate)
                throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

            if (endDate.HasValue && endDate.Value > DateTime.Now)
                throw new ArgumentException("End date cannot be in the future.", nameof(endDate));
        }
    }
} 