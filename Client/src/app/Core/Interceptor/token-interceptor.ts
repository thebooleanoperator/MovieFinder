import { HttpInterceptor, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { AuthService } from '../Services/auth-service';
import { HttpRequest, HttpHandler } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { Router } from '@angular/router';
import { switchMap, catchError } from 'rxjs/operators'
import { Injectable } from '@angular/core';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
    constructor(public authService: AuthService, private router: Router){}
    refreshingToken: boolean;

    private _refreshSubject: Subject<any> = new Subject<any>();

    private tokenExpired(): Subject<any> {
        this._refreshSubject.subscribe({
            complete: () => {
                this._refreshSubject = new Subject<any>();
            }
        });
        if (this._refreshSubject.observers.length === 1) {
            this.authService.refreshToken().subscribe(this._refreshSubject);
        }
        return this._refreshSubject;
      }

    public intercept(request: HttpRequest<any>,  next: HttpHandler): Observable<HttpEvent<any>> {
        request = this.updateHeader(request);
        // We do not want to catch error and resend for refresh token, login, or register.
        // Refresh token is not being sent for these reqeusts, so this will prevent infinte loop.
        if (request.url.endsWith("/RefreshToken") || request.url.endsWith("/Login") || request.url.endsWith("/Register")) {
            return next.handle(request);
        }
        else {
            return next.handle(request).pipe(
                catchError((error, caught) => {
                    if (error instanceof HttpErrorResponse) {
                        if (error.status == 401) {
                            return this.tokenExpired().pipe(
                                switchMap(() => {
                                  return next.handle(this.updateHeader(request));
                                })
                              );
                        }
                    }
                    return caught;
                })
            )
        }
    }

    updateHeader(request) {
        request = request.clone({
            withCredentials: true, // Needed to allow access to send cookies.
            setHeaders: {
                Authorization: `Bearer ${this.authService.token}`
            }
        });
        return request;
    }
}
