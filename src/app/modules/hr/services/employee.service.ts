import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { map, catchError, finalize, tap, switchMap } from 'rxjs/operators';
import { ApiService } from '../../../core/services/api.service';
import { StateService } from '../../../core/services/state.service';
import { MockDatabaseService } from '../../../core/services/mock-database.service';
import { Employee, EmployeeRole, EmployeeStatus } from '../models/employee.model';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private endpoint = 'employees';
  private employeesSubject = new BehaviorSubject<Employee[]>([]);
  public employees$ = this.employeesSubject.asObservable();

  constructor(
    private apiService: ApiService,
    private stateService: StateService,
    private mockDatabaseService: MockDatabaseService
  ) {
    this.loadEmployees();
  }

  /**
   * Load all employees from the centralized database
   */
  private loadEmployees(): void {
    if (environment.useMockData) {
      this.mockDatabaseService.employees$.subscribe(employees => {
        this.employeesSubject.next(employees);
        this.stateService.updateEmployees(employees);
      });
    }
  }

  getAllEmployees(): Observable<Employee[]> {
    this.stateService.setLoading(true);

    if (environment.useMockData) {
      return this.mockDatabaseService.getEmployees().pipe(
        map(employees => {
          this.stateService.updateEmployees(employees);
          return employees;
        }),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    } else {
      return this.apiService.get<Employee[]>(this.endpoint).pipe(
        map(response => {
          this.stateService.updateEmployees(response.data);
          return response.data;
        }),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    }
  }

  getEmployeeById(id: string): Observable<Employee | null> {
    this.stateService.setLoading(true);

    if (environment.useMockData) {
      return this.mockDatabaseService.getEmployeeById(id).pipe(
        map(employee => employee || null),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    } else {
      return this.apiService.get<Employee>(`${this.endpoint}/${id}`).pipe(
        map(response => response.data),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    }
  }

  createEmployee(employee: Partial<Employee>): Observable<Employee | null> {
    this.stateService.setLoading(true);

    if (environment.useMockData) {
      return this.mockDatabaseService.createEmployee(employee).pipe(
        map(newEmployee => {
          const currentEmployees = this.stateService.employees$();
          this.stateService.updateEmployees([...currentEmployees, newEmployee]);
          return newEmployee;
        }),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    } else {
      return this.apiService.post<Employee>(this.endpoint, employee).pipe(
        map(response => {
          const newEmployee = response.data;
          const currentEmployees = this.stateService.employees$();
          this.stateService.updateEmployees([...currentEmployees, newEmployee]);
          return newEmployee;
        }),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    }
  }

  updateEmployee(id: string, employee: Partial<Employee>): Observable<Employee | null> {
    this.stateService.setLoading(true);

    if (environment.useMockData) {
      return this.mockDatabaseService.updateEmployee(id, employee).pipe(
        map(updatedEmployee => {
          const currentEmployees = this.stateService.employees$();
          const updatedEmployees = currentEmployees.map(emp => 
            emp.id === id ? updatedEmployee : emp
          );
          this.stateService.updateEmployees(updatedEmployees);
          return updatedEmployee;
        }),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    } else {
      return this.apiService.put<Employee>(`${this.endpoint}/${id}`, employee).pipe(
        map(response => {
          const updatedEmployee = response.data;
          const currentEmployees = this.stateService.employees$();
          const updatedEmployees = currentEmployees.map(emp => 
            emp.id === id ? updatedEmployee : emp
          );
          this.stateService.updateEmployees(updatedEmployees);
          return updatedEmployee;
        }),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    }
  }

  deleteEmployee(id: string): Observable<boolean> {
    this.stateService.setLoading(true);

    if (environment.useMockData) {
      return this.mockDatabaseService.deleteEmployee(id).pipe(
        map(() => {
          const currentEmployees = this.stateService.employees$();
          const updatedEmployees = currentEmployees.filter(emp => emp.id !== id);
          this.stateService.updateEmployees(updatedEmployees);
          return true;
        }),
        catchError(error => {
          this.stateService.setError(error.message);
          return of(false);
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    } else {
      return this.apiService.delete<Employee>(`${this.endpoint}/${id}`).pipe(
        map(() => {
          const currentEmployees = this.stateService.employees$();
          const updatedEmployees = currentEmployees.filter(emp => emp.id !== id);
          this.stateService.updateEmployees(updatedEmployees);
          return true;
        }),
        catchError(error => {
          this.stateService.setError(error.message);
          return of(false);
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    }
  }

  updateEmployeeRole(id: string, role: EmployeeRole): Observable<Employee | null> {
    return this.updateEmployee(id, { role });
  }

  updateEmployeeStatus(id: string, status: EmployeeStatus): Observable<Employee | null> {
    return this.updateEmployee(id, { status });
  }

  getEmployeesByDepartment(department: string): Observable<Employee[]> {
    this.stateService.setLoading(true);

    if (environment.useMockData) {
      return this.mockDatabaseService.getEmployees().pipe(
        map(employees => employees.filter(emp => emp.department === department)),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    } else {
      return this.apiService.get<Employee[]>(`${this.endpoint}/department/${department}`).pipe(
        map(response => response.data),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    }
  }

  getEmployeesByRole(role: EmployeeRole): Observable<Employee[]> {
    this.stateService.setLoading(true);

    if (environment.useMockData) {
      return this.mockDatabaseService.getEmployees().pipe(
        map(employees => employees.filter(emp => emp.role === role)),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    } else {
      return this.apiService.get<Employee[]>(`${this.endpoint}/role/${role}`).pipe(
        map(response => response.data),
        catchError(error => {
          this.stateService.setError(error.message);
          return [];
        }),
        finalize(() => {
          this.stateService.setLoading(false);
        })
      );
    }
  }

  /**
   * Get active employees
   */
  getActiveEmployees(): Observable<Employee[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getEmployees().pipe(
        map(employees => employees.filter(emp => emp.status === EmployeeStatus.Active))
      );
    } else {
      return this.apiService.get<Employee[]>(`${this.endpoint}?status=active`).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Search employees by name or email
   */
  searchEmployees(query: string): Observable<Employee[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getEmployees().pipe(
        map(employees => employees.filter(emp => 
          (`${emp.firstName} ${emp.lastName}`.toLowerCase().includes(query.toLowerCase()) ||
           emp.email.toLowerCase().includes(query.toLowerCase())
          )
        ))
      );
    } else {
      return this.apiService.get<Employee[]>(`${this.endpoint}?search=${encodeURIComponent(query)}`).pipe(
        map(response => response.data)
      );
    }
  }
} 