import { Component } from '@angular/core';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { FormControl, Validators } from '@angular/forms';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { AuthDto } from 'src/app/Data/Interfaces/auth.dto';
import { Router } from '@angular/router';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
    constructor(private _authService: AuthService, private _toolBarService: ToolBarService, private _router: Router){}
    //Data
    firstName: FormControl = new FormControl('', [Validators.required]);
    lastName: FormControl = new FormControl('', [Validators.required]);
    email: FormControl = new FormControl('', [Validators.required, Validators.email]); 
    password: FormControl = new FormControl('', [Validators.required]); 
    hide: boolean = true;

    //Methods
    registerUser(firstName, lastName, email, password) {
        this._toolBarService.isLoading = true;
        this._authService.register(firstName, lastName, email, password)
            .subscribe(
                (authDto: AuthDto) => {
                    this._authService.token = authDto.token;
                    this._authService.user = authDto.userDto;
                    this._router.navigate(['/dashboard']); 
                }
            );
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