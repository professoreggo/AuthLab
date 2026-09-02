import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';


// interceptor automatically attaching the token to every reqest
export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const authService = inject(AuthService);
  const router = inject(Router);
  const token = authService.getToken();

  const clonedRequest = token
    ? req.clone({setHeaders:{Authorization: `Bearer ${token}`}})
    :req;

  return next(clonedRequest).pipe(
    catchError((error)=>{
      if ( error.status === 401){  // catch if unauthorized then delete the invalide token then naviagte to '/login'
        authService.logout();
        router.navigate(['/login']);

      }
      return throwError(()=>error);
    })
  );
};
