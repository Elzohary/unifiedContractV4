using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Enums.HR;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public enum TrainingStatus
    {
        Planned,
        InProgress,
        Completed,
        Cancelled,
        Failed
    }
    
    public class Training : BaseEntity
    {
        private string _title;
        private string _description;
        private string _provider;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _durationHours;
        private string _location;
        private decimal _cost;
        private string _currency;
        private TrainingStatus _status;
        private string _certificateUrl;
        private int? _score;
        private string _feedback;
        
        // Foreign Key
        public Guid EmployeeId { get; private set; }
        
        // Navigation Property
        public virtual Employee Employee { get; private set; }
        
        // Public properties with private setters
        public string Title => _title;
        public string Description => _description;
        public string Provider => _provider;
        public DateTime StartDate => _startDate;
        public DateTime EndDate => _endDate;
        public int DurationHours => _durationHours;
        public string Location => _location;
        public decimal Cost => _cost;
        public string Currency => _currency;
        public TrainingStatus Status => _status;
        public string CertificateUrl => _certificateUrl;
        public int? Score => _score;
        public string Feedback => _feedback;
        
        // Required by EF Core
        private Training() { }
        
        public Training(
            Guid employeeId,
            string title,
            string provider,
            DateTime startDate,
            DateTime endDate,
            int durationHours,
            string location = null,
            decimal cost = 0,
            string currency = "USD",
            string description = null)
        {
            ValidateTraining(title, provider, startDate, endDate, durationHours, currency);
            
            EmployeeId = employeeId;
            _title = title;
            _provider = provider;
            _startDate = startDate;
            _endDate = endDate;
            _durationHours = durationHours;
            _location = location;
            _cost = cost;
            _currency = currency;
            _description = description;
            _status = TrainingStatus.Planned;
            
            AddDomainEvent(new TrainingAddedEvent(this));
        }
        
        public void UpdateDetails(
            string title = null,
            string provider = null,
            string description = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? durationHours = null,
            string location = null,
            decimal? cost = null,
            string currency = null)
        {
            bool changed = false;
            
            if (title != null && title != _title)
            {
                ValidateTitle(title);
                _title = title;
                changed = true;
            }
            
            if (provider != null && provider != _provider)
            {
                ValidateProvider(provider);
                _provider = provider;
                changed = true;
            }
            
            if (description != null && description != _description)
            {
                _description = description;
                changed = true;
            }
            
            DateTime newStartDate = startDate ?? _startDate;
            DateTime newEndDate = endDate ?? _endDate;
            
            if (startDate.HasValue && startDate != _startDate)
            {
                ValidateDateRange(newStartDate, newEndDate);
                _startDate = newStartDate;
                changed = true;
            }
            
            if (endDate.HasValue && endDate != _endDate)
            {
                ValidateDateRange(newStartDate, newEndDate);
                _endDate = newEndDate;
                changed = true;
            }
            
            if (durationHours.HasValue && durationHours != _durationHours)
            {
                ValidateDurationHours(durationHours.Value);
                _durationHours = durationHours.Value;
                changed = true;
            }
            
            if (location != null && location != _location)
            {
                _location = location;
                changed = true;
            }
            
            if (cost.HasValue && cost != _cost)
            {
                ValidateCost(cost.Value);
                _cost = cost.Value;
                changed = true;
            }
            
            if (currency != null && currency != _currency)
            {
                ValidateCurrency(currency);
                _currency = currency;
                changed = true;
            }
            
            if (changed)
            {
                AddDomainEvent(new TrainingUpdatedEvent(this));
            }
        }
        
        public void StartTraining()
        {
            if (_status != TrainingStatus.Planned)
            {
                throw new InvalidOperationException("Training can only be started from Planned status.");
            }
            
            _status = TrainingStatus.InProgress;
            AddDomainEvent(new TrainingStartedEvent(this));
        }
        
        public void CompleteTraining(int? score = null, string certificateUrl = null)
        {
            if (_status != TrainingStatus.InProgress)
            {
                throw new InvalidOperationException("Training can only be completed from In Progress status.");
            }
            
            _status = TrainingStatus.Completed;
            _score = score;
            _certificateUrl = certificateUrl;
            
            AddDomainEvent(new TrainingCompletedEvent(this));
        }
        
        public void CancelTraining(string feedback = null)
        {
            if (_status == TrainingStatus.Completed || _status == TrainingStatus.Failed)
            {
                throw new InvalidOperationException("Training that is already completed or failed cannot be cancelled.");
            }
            
            _status = TrainingStatus.Cancelled;
            _feedback = feedback;
            
            AddDomainEvent(new TrainingCancelledEvent(this));
        }
        
        public void FailTraining(string feedback = null)
        {
            if (_status != TrainingStatus.InProgress)
            {
                throw new InvalidOperationException("Training can only be failed from In Progress status.");
            }
            
            _status = TrainingStatus.Failed;
            _feedback = feedback;
            
            AddDomainEvent(new TrainingFailedEvent(this));
        }
        
        public void AddFeedback(string feedback)
        {
            if (string.IsNullOrWhiteSpace(feedback))
            {
                throw new ArgumentException("Feedback cannot be empty.", nameof(feedback));
            }
            
            _feedback = feedback;
            AddDomainEvent(new TrainingFeedbackAddedEvent(this));
        }
        
        public void AddCertificate(string certificateUrl)
        {
            if (string.IsNullOrWhiteSpace(certificateUrl))
            {
                throw new ArgumentException("Certificate URL cannot be empty.", nameof(certificateUrl));
            }
            
            _certificateUrl = certificateUrl;
            AddDomainEvent(new TrainingCertificateAddedEvent(this));
        }
        
        public void SetScore(int score)
        {
            if (score < 0 || score > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 100.");
            }
            
            _score = score;
            AddDomainEvent(new TrainingScoreUpdatedEvent(this));
        }
        
        public bool IsActive()
        {
            return _status == TrainingStatus.Planned || _status == TrainingStatus.InProgress;
        }
        
        private void ValidateTraining(string title, string provider, DateTime startDate, DateTime endDate, int durationHours, string currency)
        {
            ValidateTitle(title);
            ValidateProvider(provider);
            ValidateDateRange(startDate, endDate);
            ValidateDurationHours(durationHours);
            ValidateCurrency(currency);
        }
        
        private void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Training title is required.", nameof(title));
            }
            
            if (title.Length > 200)
            {
                throw new ArgumentException("Training title cannot exceed 200 characters.", nameof(title));
            }
        }
        
        private void ValidateProvider(string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                throw new ArgumentException("Training provider is required.", nameof(provider));
            }
            
            if (provider.Length > 100)
            {
                throw new ArgumentException("Training provider cannot exceed 100 characters.", nameof(provider));
            }
        }
        
        private void ValidateDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate == default)
            {
                throw new ArgumentException("Start date must be set.", nameof(startDate));
            }
            
            if (endDate == default)
            {
                throw new ArgumentException("End date must be set.", nameof(endDate));
            }
            
            if (endDate < startDate)
            {
                throw new ArgumentException("End date cannot be before start date.", nameof(endDate));
            }
        }
        
        private void ValidateDurationHours(int durationHours)
        {
            if (durationHours <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(durationHours), "Duration hours must be greater than zero.");
            }
        }
        
        private void ValidateCost(decimal cost)
        {
            if (cost < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cost), "Cost cannot be negative.");
            }
        }
        
        private void ValidateCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException("Currency is required.", nameof(currency));
            }
            
            if (currency.Length != 3)
            {
                throw new ArgumentException("Currency should be a 3-letter ISO currency code.", nameof(currency));
            }
        }
    }
} 