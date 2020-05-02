import { Component } from '@angular/core';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
    constructor(private authService: AuthService){}
    //Data
    firstName: FormControl = new FormControl('', [Validators.required]);
    lastName: FormControl = new FormControl('', [Validators.required]);
    email: FormControl = new FormControl('', [Validators.required, Validators.email]); 
    password: FormControl = new FormControl('', [Validators.required]); 
    isLoading: boolean = false;
    hide: boolean = true;

    //Methods
    registerUser(firstName, lastName, email, password) {
        this.isLoading = true;
        this.authService.register(firstName, lastName, email, password)
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

    getNameErrorMessage(name) {
        switch(name) {
            case 'firstName':
                return this.firstName.hasError('required') ? 'You must enter a value' : ''; 
            
            case 'lastName':
                return this.lastName.hasError('required') ? 'You must enter a value' : ''; 
        }
    }
}