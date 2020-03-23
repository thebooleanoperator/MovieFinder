import { Component } from '@angular/core';
import { AuthService } from 'src/app/Services/auth-service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
    constructor(private authService: AuthService){}
    //Data
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    isLoading: boolean = false;

    //Methods
    registerUser(firstName, lastName, email, password) {
        this.isLoading = true;
        this.authService.register(firstName, lastName, email, password)
            .catch((error) => {
                alert(error.error);
            })
            .finally(() => this.isLoading = false);
    }
}