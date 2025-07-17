import { Injectable } from '@angular/core';
import { CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService, AuthUser } from '../../../shared/services/auth.service';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class HrRoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): Observable<boolean | UrlTree> {
    return this.authService.currentUser$.pipe(
      map((user: AuthUser | null) => {
        if (!user) {
          return this.router.createUrlTree(['/login']);
        }
        // Allow if user is administrator
        if (user.role === 'administrator') {
          return true;
        }
        // Allow if user has any HR-related permission
        if (user.permissions && user.permissions.some(p => p.toLowerCase().includes('hr'))) {
          return true;
        }
        // If HR is added to AuthUser.role in the future, add: if (user.role === 'HR') { return true; }
        // Redirect to not-authorized page or home
        return this.router.createUrlTree(['/not-authorized']);
      })
    );
  }
} 