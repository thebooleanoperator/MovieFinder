import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { LoginService } from '../Services/login.service';

//This class needs to be re written once the real authentication is in place. 
@Injectable({ providedIn: 'root' })
export class checkLogin implements CanActivate {
    constructor(private LoginService: LoginService, private router: Router){}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot){
        const loggedIn = this.LoginService.userLoggedIn();

        if (loggedIn) {
            return true; 
        }

        this.router.navigate(['/welcome'])
        return false; 
    }
}