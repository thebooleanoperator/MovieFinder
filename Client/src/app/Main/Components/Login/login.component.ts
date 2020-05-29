import { Component } from '@angular/core';
import { AuthService } from '../../../Core/Services/auth-service';
import { FormControl, Validators } from '@angular/forms';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { Router } from '@angular/router';
import { AuthDto } from 'src/app/Data/Interfaces/auth.dto';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent {
    constructor(private authService: AuthService, private _toolBarService: ToolBarService, private _router: Router){}
    //Data
    email: FormControl = new FormControl('', [Validators.required, Validators.email]); 
    password: FormControl = new FormControl('', [Validators.required]); 
    hide: boolean = true;

    //Methods
    verifyUserAndLogin(email, password) {
        this._toolBarService.isLoading = true;
        this.authService.login(email, password)
            .subscribe(
                (authDto: AuthDto) => {
                    this.authService.token = authDto.token;
                    this.authService.user = authDto.userDto;
                    this.authService.setRefreshToken(authDto.refreshToken);
                    this._router.navigate(['/dashboard']);
                },
                (error) => {
                    alert(error.error);
                    this._toolBarService.isLoading = false;
                },
                () => this._toolBarService.isLoading = false
            )
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