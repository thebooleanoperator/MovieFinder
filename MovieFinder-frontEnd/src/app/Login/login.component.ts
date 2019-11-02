import { Component } from '@angular/core';
import { LoginService } from '../Services/login.service';
import { Router } from '@angular/router';
import { UserService } from '../Services/user.service';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    providers: [LoginService]
})

export class LoginComponent {
    email: string; 
    password: string; 
    
    constructor(private loginService: LoginService, private router: Router, private userService: UserService){}


    verifyUserAndLogin(email, password) {
        this.loginService.login(email, password)
            .subscribe((response) => {
                if (response) {
                    this.userService.setUser(response);
                    this.router.navigate(['home']);
                }
                else {
                    console.log('failure');
                }
            }); 
    }
}