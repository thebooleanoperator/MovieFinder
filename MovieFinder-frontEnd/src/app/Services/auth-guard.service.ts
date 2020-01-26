import { CanActivate, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
    constructor(private router: Router)
    {

    }

    canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return true;
    }
}