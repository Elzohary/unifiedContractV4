import { Injectable } from '@angular/core';
import { Observable, of, catchError, map, tap, BehaviorSubject, shareReplay, forkJoin, switchMap, throwError, combineLatest, finalize, first, share } from 'rxjs';
import { CacheService } from './cache.service';
import { EmployeeService } from './employee.service';
import { 
  Employee, 
  EmployeeRequest, 
  AttendanceRecord, 
  Warning, 
  SickLeave, 
  Announcement 
} from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class DataRepositoryService {
  private dataLoadingSubject = new BehaviorSubject<boolean>(false);
  
  // Expose an observable to track loading state across the app
  public dataLoading$ = this.dataLoadingSubject.asObservable();
  
  // Caching durations in milliseconds
  private CACHE_DURATIONS = {
    EMPLOYEES: 5 * 60 * 1000, // 5 minutes
    REQUESTS: 2 * 60 * 1000,  // 2 minutes 
    ATTENDANCE: 5 * 60 * 1000, // 5 minutes
    WARNINGS: 5 * 60 * 1000,  // 5 minutes
    ANNOUNCEMENTS: 10 * 60 * 1000, // 10 minutes
    OTHER: 2 * 60 * 1000     // 2 minutes for other data
  };

  constructor(
    private cacheService: CacheService,
    private employeeService: EmployeeService
  ) {}

  private updateLastLoaded(dataType: string): void {
    // This method updates the timestamp for when data was last loaded
    console.log(`Data type ${dataType} last loaded at: ${new Date().toLocaleString()}`);
  }

  /**
   * Get employees (live, no cache)
   */
  getEmployees(): Observable<Employee[]> {
    return this.employeeService.getEmployees();
  }

  /**
   * Get employee by ID (live, no cache)
   */
  getEmployeeById(id: string): Observable<Employee | undefined> {
    return this.employeeService.getEmployeeById(id);
  }

  /**
   * Get employee with all related data in a single call (live, no cache)
   */
  getEmployeeWithRelatedData(id: string): Observable<{
    employee: Employee | undefined,
    attendance: AttendanceRecord[],
    requests: EmployeeRequest[],
    warnings: Warning[]
  }> {
    return this.employeeService.getEmployeeById(id).pipe(
      first(),
      switchMap(employee => {
        if (!employee) {
          return of({
            employee: undefined,
            attendance: [] as AttendanceRecord[],
            requests: [] as EmployeeRequest[],
            warnings: [] as Warning[]
          });
        }
        const attendance$ = this.employeeService.getAttendanceByEmployee(id).pipe(first());
        const requests$ = this.employeeService.getRequestsByEmployee(id).pipe(first());
        const warnings$ = this.employeeService.getWarningsByEmployee(id).pipe(first());
        return forkJoin({
          employee: of(employee),
          attendance: attendance$,
          requests: requests$,
          warnings: warnings$
        });
      })
    );
  }

  /**
   * Get attendance records by employee (live, no cache)
   */
  getAttendanceByEmployee(employeeId: string): Observable<AttendanceRecord[]> {
    return this.employeeService.getAttendanceByEmployee(employeeId);
  }

  getAttendanceRecords(startDate?: string, endDate?: string): Observable<AttendanceRecord[]> {
    return this.employeeService.getAttendanceRecords(startDate, endDate);
  }

  /**
   * Get requests by employee (live, no cache)
   */
  getRequestsByEmployee(employeeId: string): Observable<EmployeeRequest[]> {
    return this.employeeService.getRequestsByEmployee(employeeId);
  }

  /**
   * Get warnings by employee (live, no cache)
   */
  getWarningsByEmployee(employeeId: string): Observable<Warning[]> {
    return this.employeeService.getWarningsByEmployee(employeeId);
  }

  /**
   * Get active announcements (live, no cache)
   */
  getActiveAnnouncements(): Observable<Announcement[]> {
    return this.employeeService.getActiveAnnouncements();
  }

  // --- Employee CRUD ---
  addEmployee(employee: Employee): Observable<Employee> {
    return this.employeeService.addEmployee(employee).pipe(
      tap(() => this.invalidateCache('employees'))
    );
  }

  updateEmployee(employee: Employee): Observable<Employee> {
    return this.employeeService.updateEmployee(employee).pipe(
      tap(() => this.invalidateCache('employee', employee.id))
    );
  }

  deleteEmployee(id: string): Observable<boolean> {
    return this.employeeService.deleteEmployee(id).pipe(
      tap(() => this.invalidateCache('employee', id))
    );
  }

  // --- Employee Requests ---
  addEmployeeRequest(employeeId: string, request: EmployeeRequest): Observable<EmployeeRequest> {
    return this.employeeService.addEmployeeRequest(employeeId, request).pipe(
      tap(() => this.invalidateCache('requests', employeeId))
    );
  }

  updateEmployeeRequest(employeeId: string, request: EmployeeRequest): Observable<EmployeeRequest> {
    return this.employeeService.updateEmployeeRequest(employeeId, request).pipe(
      tap(() => this.invalidateCache('requests', employeeId))
    );
  }

  deleteEmployeeRequest(employeeId: string, requestId: string): Observable<boolean> {
    return this.employeeService.deleteEmployeeRequest(employeeId, requestId).pipe(
      tap(() => this.invalidateCache('requests', employeeId))
    );
  }

  // --- Employee Attendance ---
  addEmployeeAttendance(employeeId: string, record: AttendanceRecord): Observable<AttendanceRecord> {
    return this.employeeService.addEmployeeAttendance(employeeId, record).pipe(
      tap(() => this.invalidateCache('attendance', employeeId))
    );
  }

  updateEmployeeAttendance(employeeId: string, record: AttendanceRecord): Observable<AttendanceRecord> {
    return this.employeeService.updateEmployeeAttendance(employeeId, record).pipe(
      tap(() => this.invalidateCache('attendance', employeeId))
    );
  }

  deleteEmployeeAttendance(employeeId: string, recordId: string): Observable<boolean> {
    return this.employeeService.deleteEmployeeAttendance(employeeId, recordId).pipe(
      tap(() => this.invalidateCache('attendance', employeeId))
    );
  }

  getAllAttendanceRecords(): AttendanceRecord[] {
    return this.employeeService.getAttendanceSubjectValue();
  }
  setAllAttendanceRecords(records: AttendanceRecord[]): void {
    this.employeeService.setAttendanceRecords(records);
  }

  /**
   * Clear cache when data is updated
   */
  invalidateCache(type: 'employees' | 'employee' | 'attendance' | 'requests' | 'warnings' | 'announcements', id?: string): void {
    switch (type) {
      case 'employees':
        this.cacheService.clear('employees');
        this.cacheService.clear('employee_');
        break;
      case 'employee':
        if (id) {
          this.cacheService.remove(`employee_${id}`);
          this.cacheService.remove(`employee_full_${id}`);
        }
        this.cacheService.remove('employees');
        break;
      case 'attendance':
        if (id) {
          this.cacheService.remove(`attendance_${id}`);
          this.cacheService.remove(`employee_full_${id}`);
        }
        break;
      case 'requests':
        if (id) {
          this.cacheService.remove(`requests_${id}`);
          this.cacheService.remove(`employee_full_${id}`);
        }
        break;
      case 'warnings':
        if (id) {
          this.cacheService.remove(`warnings_${id}`);
          this.cacheService.remove(`employee_full_${id}`);
        }
        break;
      case 'announcements':
        this.cacheService.remove('active_announcements');
        break;
    }
  }
} 