import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from './auth-service';

@Injectable({providedIn: "root"})
export class AuthGuardService implements CanActivate {
    constructor(private router: Router, private authService: AuthService) 
    {    
    }

    canActivate(): boolean {
        if (!this.authService.isLoggedIn()) {
            this.router.navigate(['/welcome']);
            alert("Your session has ended. Redirecting to welcome page.")
            return false;
        }
        return true;
    }
}