import { Component } from '@angular/core';
import { SignInService } from '../../../Services/sign-in.service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent {
    constructor(private signInService: SignInService){}
    //Data
    email: string; 
    password: string; 
    isLoading: boolean = false;

    //Methods
    verifyUserAndLogin(email, password) {
        this.isLoading = true;
        this.signInService.login(email, password)
            .finally(() => this.isLoading = false);
    }
}