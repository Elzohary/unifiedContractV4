using System;
using UnifiedContract.Domain.Common;
using UnifiedContract.Domain.Events.HR;

namespace UnifiedContract.Domain.Entities.HR
{
    public class Attendance : BaseEntity
    {
        private DateTime _date;
        private DateTime? _checkInTime;
        private DateTime? _checkOutTime;
        private TimeSpan? _workingHours;
        private TimeSpan? _lateMinutes;
        private TimeSpan? _earlyDepartureMinutes;
        private TimeSpan? _overtimeHours;
        private string _notes;
        private bool _isAbsent;
        private bool _isHalfDay;
        private bool _isOnLeave;
        private Guid? _leaveId;
        private string _checkInLocation;
        private string _checkOutLocation;
        
        // Foreign Keys
        public Guid EmployeeId { get; private set; }
        
        // Navigation Properties
        public virtual Employee Employee { get; private set; }
        public virtual Leave Leave { get; private set; }
        
        // Public properties with private setters
        public DateTime Date => _date;
        public DateTime? CheckInTime => _checkInTime;
        public DateTime? CheckOutTime => _checkOutTime;
        public TimeSpan? WorkingHours => _workingHours;
        public TimeSpan? LateMinutes => _lateMinutes;
        public TimeSpan? EarlyDepartureMinutes => _earlyDepartureMinutes;
        public TimeSpan? OvertimeHours => _overtimeHours;
        public string Notes => _notes;
        public bool IsAbsent => _isAbsent;
        public bool IsHalfDay => _isHalfDay;
        public bool IsOnLeave => _isOnLeave;
        public Guid? LeaveId => _leaveId;
        public string CheckInLocation => _checkInLocation;
        public string CheckOutLocation => _checkOutLocation;
        
        // Required by EF Core
        private Attendance() { }
        
        public Attendance(
            Guid employeeId,
            DateTime date,
            bool isAbsent = false,
            bool isHalfDay = false,
            bool isOnLeave = false,
            Guid? leaveId = null,
            string notes = null)
        {
            if (date == default)
            {
                throw new ArgumentException("Date must be set.", nameof(date));
            }
            
            if (date.Date > DateTime.Today)
            {
                throw new ArgumentException("Cannot record attendance for future dates.", nameof(date));
            }
            
            if (isOnLeave && !leaveId.HasValue)
            {
                throw new ArgumentException("Leave ID is required when marking attendance as on leave.", nameof(leaveId));
            }
            
            EmployeeId = employeeId;
            _date = date.Date; // Store only the date part
            _isAbsent = isAbsent;
            _isHalfDay = isHalfDay;
            _isOnLeave = isOnLeave;
            _leaveId = leaveId;
            _notes = notes;
            
            AddDomainEvent(new AttendanceCreatedEvent(this));
        }
        
        public void RecordCheckIn(DateTime checkInTime, string location = null)
        {
            if (_isAbsent)
            {
                throw new InvalidOperationException("Cannot check in when marked as absent.");
            }
            
            if (_isOnLeave)
            {
                throw new InvalidOperationException("Cannot check in when on leave.");
            }
            
            if (_checkInTime.HasValue)
            {
                throw new InvalidOperationException("Check-in already recorded.");
            }
            
            if (checkInTime.Date != _date)
            {
                throw new ArgumentException("Check-in time must be on the same date as the attendance record.", nameof(checkInTime));
            }
            
            _checkInTime = checkInTime;
            _checkInLocation = location;
            _isAbsent = false; // Ensure not marked as absent
            
            CalculateLateMinutes();
            
            AddDomainEvent(new AttendanceCheckedInEvent(this));
        }
        
        public void RecordCheckOut(DateTime checkOutTime, string location = null)
        {
            if (!_checkInTime.HasValue)
            {
                throw new InvalidOperationException("Cannot check out before checking in.");
            }
            
            if (_checkOutTime.HasValue)
            {
                throw new InvalidOperationException("Check-out already recorded.");
            }
            
            if (checkOutTime < _checkInTime)
            {
                throw new ArgumentException("Check-out time cannot be before check-in time.", nameof(checkOutTime));
            }
            
            if (checkOutTime.Date != _date)
            {
                throw new ArgumentException("Check-out time must be on the same date as the attendance record.", nameof(checkOutTime));
            }
            
            _checkOutTime = checkOutTime;
            _checkOutLocation = location;
            
            CalculateWorkingHours();
            CalculateEarlyDepartureMinutes();
            CalculateOvertimeHours();
            
            AddDomainEvent(new AttendanceCheckedOutEvent(this));
        }
        
        public void MarkAsAbsent(string notes = null)
        {
            if (_isOnLeave)
            {
                throw new InvalidOperationException("Cannot mark as absent when on leave.");
            }
            
            if (_checkInTime.HasValue || _checkOutTime.HasValue)
            {
                throw new InvalidOperationException("Cannot mark as absent when check-in or check-out has been recorded.");
            }
            
            _isAbsent = true;
            _isHalfDay = false;
            
            if (notes != null)
            {
                _notes = notes;
            }
            
            AddDomainEvent(new AttendanceMarkedAsAbsentEvent(this));
        }
        
        public void MarkAsHalfDay(string notes = null)
        {
            if (_isOnLeave)
            {
                throw new InvalidOperationException("Cannot mark as half day when on leave.");
            }
            
            _isHalfDay = true;
            _isAbsent = false;
            
            if (notes != null)
            {
                _notes = notes;
            }
            
            AddDomainEvent(new AttendanceMarkedAsHalfDayEvent(this));
        }
        
        public void MarkAsOnLeave(Guid leaveId, string notes = null)
        {
            if (_isAbsent)
            {
                throw new InvalidOperationException("Cannot mark as on leave when marked as absent.");
            }
            
            if (_checkInTime.HasValue || _checkOutTime.HasValue)
            {
                throw new InvalidOperationException("Cannot mark as on leave when check-in or check-out has been recorded.");
            }
            
            _isOnLeave = true;
            _isAbsent = false;
            _isHalfDay = false;
            _leaveId = leaveId;
            
            if (notes != null)
            {
                _notes = notes;
            }
            
            AddDomainEvent(new AttendanceMarkedAsOnLeaveEvent(this));
        }
        
        public void AddNotes(string notes)
        {
            if (string.IsNullOrWhiteSpace(notes))
            {
                throw new ArgumentException("Notes cannot be empty.", nameof(notes));
            }
            
            _notes = notes;
            AddDomainEvent(new AttendanceNotesUpdatedEvent(this));
        }
        
        private void CalculateWorkingHours()
        {
            if (_checkInTime.HasValue && _checkOutTime.HasValue)
            {
                _workingHours = _checkOutTime.Value - _checkInTime.Value;
            }
        }
        
        private void CalculateLateMinutes()
        {
            if (_checkInTime.HasValue)
            {
                // Assuming standard work start time is 9:00 AM
                var workStartTime = new DateTime(_date.Year, _date.Month, _date.Day, 9, 0, 0);
                
                if (_checkInTime > workStartTime)
                {
                    _lateMinutes = _checkInTime.Value - workStartTime;
                }
                else
                {
                    _lateMinutes = TimeSpan.Zero;
                }
            }
        }
        
        private void CalculateEarlyDepartureMinutes()
        {
            if (_checkOutTime.HasValue)
            {
                // Assuming standard work end time is 5:00 PM
                var workEndTime = new DateTime(_date.Year, _date.Month, _date.Day, 17, 0, 0);
                
                if (_checkOutTime < workEndTime)
                {
                    _earlyDepartureMinutes = workEndTime - _checkOutTime.Value;
                }
                else
                {
                    _earlyDepartureMinutes = TimeSpan.Zero;
                }
            }
        }
        
        private void CalculateOvertimeHours()
        {
            if (_checkOutTime.HasValue)
            {
                // Assuming standard work end time is 5:00 PM
                var workEndTime = new DateTime(_date.Year, _date.Month, _date.Day, 17, 0, 0);
                
                if (_checkOutTime > workEndTime)
                {
                    _overtimeHours = _checkOutTime.Value - workEndTime;
                }
                else
                {
                    _overtimeHours = TimeSpan.Zero;
                }
            }
        }
    }
} 