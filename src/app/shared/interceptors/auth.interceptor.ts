import { HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

export const authInterceptor = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const token = localStorage.getItem('auth_token');
  if (token) {
    console.log('[AUTH INTERCEPTOR] Attaching Authorization header:', token);
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  } else {
    console.log('[AUTH INTERCEPTOR] No token found in localStorage.');
  }
  return next(req);
}; 