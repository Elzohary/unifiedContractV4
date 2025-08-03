import { HttpRequest, HttpHandlerFn, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const errorInterceptor = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const router = inject(Router);
  const authService = inject(AuthService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        console.log('[ERROR INTERCEPTOR] 401 Unauthorized error detected. Clearing auth state and redirecting to login.');
        
        // Store the current URL for redirect after login (if not already on login page)
        const currentUrl = router.url;
        if (currentUrl !== '/login') {
          authService.redirectUrl = currentUrl;
        }
        
        // Clear authentication state
        authService.clearAuth();
        
        // Redirect to login page
        router.navigate(['/login']);
        
        // Return empty observable to prevent further error propagation
        return throwError(() => new Error('Unauthorized - redirected to login'));
      }
      
      // For other errors, just re-throw them
      return throwError(() => error);
    })
  );
}; 