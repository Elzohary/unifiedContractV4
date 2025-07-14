import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { map, tap, switchMap } from 'rxjs/operators';
import { MockDatabaseService } from '../../core/services/mock-database.service';
import { ApiService } from '../../core/services/api.service';
import { environment } from '../../../environments/environment';

export interface User {
  id: number;
  name: string;
  email: string;
  role: string;
  avatar?: string;
  isActive?: boolean;
  createdAt?: Date;
  lastLoginAt?: Date;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private usersSubject = new BehaviorSubject<User[]>([]);
  public users$ = this.usersSubject.asObservable();

  constructor(
    private mockDatabaseService: MockDatabaseService,
    private apiService: ApiService
  ) {
    this.loadUsers();
  }

  /**
   * Load all users from the centralized database
   */
  loadUsers(): Observable<User[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getUsers().pipe(
        tap(users => this.usersSubject.next(users))
      );
    } else {
      return this.apiService.get<User[]>('users').pipe(
        map(response => response.data),
        tap(users => this.usersSubject.next(users))
      );
    }
  }

  /**
   * Get all users
   */
  getUsers(): Observable<User[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getUsers();
    } else {
      return this.apiService.get<User[]>('users').pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Get user by ID
   */
  getUserById(id: number): Observable<User | undefined> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getUserById(id);
    } else {
      return this.apiService.get<User>(`users/${id}`).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Create a new user
   */
  createUser(user: Partial<User>): Observable<User> {
    if (environment.useMockData) {
      return this.mockDatabaseService.createUser(user).pipe(
        tap(() => this.refreshUsers())
      );
    } else {
      return this.apiService.post<User>('users', user).pipe(
        map(response => response.data),
        tap(() => this.refreshUsers())
      );
    }
  }

  /**
   * Update an existing user
   */
  updateUser(id: number, updates: Partial<User>): Observable<User> {
    if (environment.useMockData) {
      return this.mockDatabaseService.updateUser(id, updates).pipe(
        tap(() => this.refreshUsers())
      );
    } else {
      return this.apiService.put<User>(`users/${id}`, updates).pipe(
        map(response => response.data),
        tap(() => this.refreshUsers())
      );
    }
  }

  /**
   * Delete a user
   */
  deleteUser(id: number): Observable<boolean> {
    if (environment.useMockData) {
      return this.mockDatabaseService.deleteUser(id).pipe(
        tap(() => this.refreshUsers())
      );
    } else {
      return this.apiService.delete<boolean>(`users/${id}`).pipe(
        map(response => response.data),
        tap(() => this.refreshUsers())
      );
    }
  }

  /**
   * Get users by role
   */
  getUsersByRole(role: string): Observable<User[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getUsers().pipe(
        map(users => users.filter(user => user.role === role))
      );
    } else {
      return this.apiService.get<User[]>(`users?role=${role}`).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Get active users
   */
  getActiveUsers(): Observable<User[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getUsers().pipe(
        map(users => users.filter(user => user.isActive !== false))
      );
    } else {
      return this.apiService.get<User[]>('users?active=true').pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Search users by name or email
   */
  searchUsers(query: string): Observable<User[]> {
    if (environment.useMockData) {
      return this.mockDatabaseService.getUsers().pipe(
        map(users => users.filter(user => 
          user.name.toLowerCase().includes(query.toLowerCase()) ||
          user.email.toLowerCase().includes(query.toLowerCase())
        ))
      );
    } else {
      return this.apiService.get<User[]>(`users?search=${encodeURIComponent(query)}`).pipe(
        map(response => response.data)
      );
    }
  }

  /**
   * Refresh the users list from the centralized database
   */
  private refreshUsers(): void {
    this.loadUsers().subscribe();
  }
}
