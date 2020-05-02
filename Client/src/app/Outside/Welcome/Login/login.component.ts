import { Component } from '@angular/core';
import { AuthService } from '../../../Services/auth-service';
import { FormControl, Validators } from '@angular/forms';
import { ToolBarService } from 'src/app/Services/tool-bar.service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent {
    constructor(private authService: AuthService, private _toolBarService: ToolBarService){}
    //Data
    email: FormControl = new FormControl('', [Validators.required, Validators.email]); 
    password: FormControl = new FormControl('', [Validators.required]); 
    hide: boolean = true;

    //Methods
    verifyUserAndLogin(email, password) {
        this._toolBarService.isLoading = true;
        this.authService.login(email, password)
            .catch((error) => {
                alert(error.error);
            })
            .finally(() => this._toolBarService.isLoading = false);
    }
    
    getEmailErrorMessage() {
        if (this.email.hasError('required')) {
            return 'Email Required';
        }

        return this.email.hasError('email') ? 'Not a valid email' : '';
    }
    
    getPasswordErrorMessage() {
        if (this.password.hasError('required')) {
            return 'Password Required';
        }
    }
}