import { ErrorHandler, Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ActivityLogService } from '../services/activity-log.service';
import { UserService } from '../services/user.service';
import { AuthService } from '../services/auth.service';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  private router = inject(Router);
  private activityLogService = inject(ActivityLogService);
  private userService = inject(UserService);
  private authService = inject(AuthService);

  handleError(error: any): void {
    // Log the error to the console
    console.error('Global error handler caught an error:', error);

    // Log the error to the activity log service
    this.activityLogService.logErrorEvent(
      error,
      'Application Error',
      this.userService.getCurrentUserId(),
      this.userService.getCurrentUserName()
    ).subscribe();

    // Handle specific error types
    if (error.status === 401) {
      // Unauthorized - clear auth state and redirect to login
      console.log('401 Unauthorized error detected. Clearing auth state and redirecting to login.');
      this.authService.clearAuth();
      this.router.navigate(['/login']);
    } else if (error.status === 404) {
      // Not found - redirect to 404 page
      this.router.navigate(['/not-found']);
    } else if (error.status === 500) {
      // Server error - redirect to error page
      this.router.navigate(['/server-error']);
    }

    // Re-throw the error to maintain the default behavior
    throw error;
  }
} 