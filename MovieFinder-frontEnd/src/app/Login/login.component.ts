import { Component } from '@angular/core';
import { LoginService } from '../Services/login.service';
import { Router } from '@angular/router';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    providers: [LoginService]
})

export class LoginComponent {
    title: string = 'Movie Finder TM';
    email: string; 
    password: string; 
    validation: Object; 

    constructor(private LoginService: LoginService, private router: Router){}

    verifyUserAndLogin(email, password): void {
        this.LoginService.validateLogin(email, password)
            .subscribe((response) => {
                if (response == true) {
                    this.router.navigate(['home']);
                }
                else {
                    console.log('failure')
                }
            })
    }

 
}