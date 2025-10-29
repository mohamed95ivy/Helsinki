import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, throwError } from 'rxjs';


export const errorInterceptor: HttpInterceptorFn = (req, next) => {
    const snack = inject(MatSnackBar);
    return next(req).pipe(
        catchError((err: HttpErrorResponse) => {
            const msg = err.error?.title || err.message || 'Request failed';
            snack.open(msg, 'Dismiss', { duration: 4000 });
            return throwError(() => err);
        })
    );
};