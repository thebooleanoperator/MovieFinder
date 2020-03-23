import { Component } from '@angular/core';
import { AuthService } from '../../../Services/auth-service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent {
    constructor(private authService: AuthService){}
    //Data
    email: string; 
    password: string; 
    isLoading: boolean = false;

    //Methods
    verifyUserAndLogin(email, password) {
        this.isLoading = true;
        this.authService.login(email, password)
            .catch((error) => {
                alert(error.error);
            })
            .finally(() => this.isLoading = false);
    }
}