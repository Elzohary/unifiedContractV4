import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { ApiService } from '../../core/services/api.service';

export interface AuthUser {
  id: string;
  name: string;
  email: string;
  role: string;
  avatar?: string;
  permissions: string[];
  lastLogin?: Date;
  isActive: boolean;
  isEmployee: boolean;
  employeeId?: string;
}

interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<AuthUser | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();
  
  // Store the URL that was attempted to be accessed before login
  redirectUrl: string | null = null;

  constructor(private api: ApiService) {
    this.checkStoredAuth();
  }

  get isLoggedIn(): boolean {
    const token = localStorage.getItem('auth_token');
    return !!token && !!this.currentUserSubject.value;
  }

  get currentUserValue(): AuthUser | null {
    return this.currentUserSubject.value;
  }

  login(email: string, password: string): Observable<AuthUser> {
    return this.api.post<LoginResponse>('user/login', { email, password }).pipe(
      map(response => {
        // Store the token
        localStorage.setItem('auth_token', response.data.token);
        
        // Decode the JWT token to get user information
        const userInfo = this.decodeToken(response.data.token);
        
        // Set user info from token
        const user: AuthUser = {
          id: userInfo.sub || email,
          name: userInfo.name || email,
          email: userInfo.email || email,
          role: userInfo.role || 'user',
          permissions: userInfo.permissions || [], // Extract permissions from token if present
          isActive: true,
          isEmployee: false
        };
        
        this.currentUserSubject.next(user);
        return user;
      })
    );
  }

  logout(): Observable<boolean> {
    localStorage.removeItem('auth_token');
    this.currentUserSubject.next(null);
    this.redirectUrl = null; // Clear redirect URL on logout
    return new Observable<boolean>(observer => {
      observer.next(true);
      observer.complete();
    });
  }

  hasPermission(permission: string): boolean {
    const user = this.currentUserSubject.value;
    if (!user) return false;
    return user.permissions && user.permissions.includes(permission);
  }

  private decodeToken(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));
      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error('Error decoding token:', error);
      return {};
    }
  }

  private checkStoredAuth(): void {
    const token = localStorage.getItem('auth_token');
    if (token) {
      try {
        // Decode the stored token to get user information
        const userInfo = this.decodeToken(token);
        
        this.currentUserSubject.next({
          id: userInfo.sub || 'token-user',
          name: userInfo.name || 'Authenticated User',
          email: userInfo.email || '',
          role: userInfo.role || 'user',
          permissions: userInfo.permissions || [], // Extract permissions from token if present
          isActive: true,
          isEmployee: false
        });
      } catch (error) {
        console.error('Error decoding stored token:', error);
        // Fallback to basic user object
        this.currentUserSubject.next({
          id: 'token-user',
          name: 'Authenticated User',
          email: '',
          role: 'user',
          permissions: [],
          isActive: true,
          isEmployee: false
        });
      }
    }
  }

  // Method to clear auth state (useful for testing or manual logout)
  clearAuth(): void {
    localStorage.removeItem('auth_token');
    this.currentUserSubject.next(null);
    this.redirectUrl = null;
  }
} 