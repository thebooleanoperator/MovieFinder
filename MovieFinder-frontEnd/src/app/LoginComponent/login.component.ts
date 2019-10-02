import { Component } from '@angular/core';
import { LoginService } from './login.service';
import { Router } from '@angular/router';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    providers: [LoginService]
})

export class LoginComponent {
    title: string = 'Movie Finder TM';
    email: string; 
    password: string; 
    validation: Object; 

    constructor(private LoginService: LoginService, private router: Router)
    {

    }

    verifyUserAndLogin(email, password): void {
        this.LoginService.validateLogin(email, password)
        .subscribe((response) => {
            console.log(response);
            if (response == true) {
            this.Login()
            }
        })
        }

    Login(): void {
        this.router.navigate(["/welcome"]);
    }
}