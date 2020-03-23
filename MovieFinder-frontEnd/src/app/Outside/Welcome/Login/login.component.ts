import { Component } from '@angular/core';
import { AuthService } from '../../../Services/auth-service';
import { FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent {
    constructor(private authService: AuthService){}
    //Data
    email: FormControl = new FormControl('', [Validators.required, Validators.email]); 
    password: FormControl = new FormControl('', [Validators.required]); 
    isLoading: boolean = false;
    hide: boolean = true;

    //Methods
    verifyUserAndLogin(email, password) {
        this.isLoading = true;
        this.authService.login(email, password)
            .catch((error) => {
                alert(error.error);
            })
            .finally(() => this.isLoading = false);
    }
    
    getEmailErrorMessage() {
        if (this.email.hasError('required')) {
            return 'You must enter a value';
        }

        return this.email.hasError('email') ? 'Not a valid email' : '';
    }
    
    getPasswordErrorMessage() {
        if (this.password.hasError('required')) {
            return 'You must enter a value';
        }
    }
}