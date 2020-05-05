import { Component } from '@angular/core';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { FormControl, Validators } from '@angular/forms';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
    constructor(private authService: AuthService, private _toolBarService: ToolBarService){}
    //Data
    firstName: FormControl = new FormControl('', [Validators.required]);
    lastName: FormControl = new FormControl('', [Validators.required]);
    email: FormControl = new FormControl('', [Validators.required, Validators.email]); 
    password: FormControl = new FormControl('', [Validators.required]); 
    hide: boolean = true;

    //Methods
    registerUser(firstName, lastName, email, password) {
        this._toolBarService.isLoading = true;
        this.authService.register(firstName, lastName, email, password)
            .catch((error) => {
                alert(error.error);
            })
            .finally(() => this._toolBarService.isLoading = false);
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