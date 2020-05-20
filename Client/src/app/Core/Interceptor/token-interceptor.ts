import { HttpInterceptor, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { AuthService } from '../Services/auth-service';
import { HttpRequest, HttpHandler } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators'
import { Injectable } from '@angular/core';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    constructor(public authService: AuthService, private router: Router){}
    refreshingToken: boolean;

    public intercept(request: HttpRequest<any>,  next: HttpHandler): Observable<HttpEvent<any>> {
        request = request.clone({
            withCredentials: true, // Needed to allow access to send cookies.
            
            setHeaders: {
                Authorization: `Bearer ${this.authService.token}`
            }
        });
        return next.handle(request).pipe(tap(() => {}, 
            ((error: any) => {
                if (error instanceof HttpErrorResponse) {
                    if (error.status == 401) {
                        if (!this.refreshingToken) {
                            this.refreshingToken = true;
                            this.authService.refreshToken()
                            .then(() => {
                                console.log("refreshed")
                            })
                            .catch(() => {
                                this.authService.logout();
                                alert("Session has ended, logging out now.");
                            })
                            .finally(() => this.refreshingToken = false); 
                        }
                    }
                }
            })
        ));
    }
}
