import { HttpInterceptor, HttpEvent, HttpErrorResponse, HttpResponse } from "@angular/common/http";
import { SignInService } from './sign-in.service';
import { HttpRequest, HttpHandler } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators'
import { Injectable } from '@angular/core';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    constructor(public signInService: SignInService, private router: Router){}

    public intercept(request: HttpRequest<any>,  next: HttpHandler): Observable<HttpEvent<any>> {
        request = request.clone({
            setHeaders: {
                Authorization: `Bearer ${this.signInService.getToken()}`
            }
        });
        return next.handle(request).pipe(tap(() => {}, 
            ((error: any) => {
                if (error instanceof HttpErrorResponse) {
                    if (error.status == 401) {
                        this.signInService.logout();
                        alert("Session expired, you have been logged out.")
                    }
                }
            })
        ));
    }
}
