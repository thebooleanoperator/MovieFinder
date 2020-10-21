import { Component } from '@angular/core';
import { AuthService } from '../../../Core/Services/auth-service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { Router } from '@angular/router';
import { AuthDto } from 'src/app/Data/Interfaces/auth.dto';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent {
    constructor(
        private _authService: AuthService, 
        private _toolBarService: ToolBarService, 
        private _router: Router,
        private _formBuilder: FormBuilder)
    {
        this.loginForm = this._formBuilder.group
        (
            {
                userEmail: ['', [Validators.required, Validators.email]],
                password: ['', [Validators.required]]
            }
        )
    }
    //Data
    loginForm: FormGroup; 
    hide: boolean = true;

    //Methods
    verifyUserAndLogin(loginForm: FormGroup) {
        var email = loginForm.controls.userEmail.value; 
        var password = loginForm.controls.password.value;
        
        this._toolBarService.isLoading = true;
        this._authService.login(email, password)
            .subscribe(
                (authDto: AuthDto) => {
                    this._authService.token = authDto.token;
                    this._authService.user = authDto.userDto;
                    this._router.navigate(['/content/dashboard'])
                        .finally(() => this._toolBarService.isLoading = false);
                },
                (error) => {
                    alert(error.error);
                    this._toolBarService.isLoading = false;
                }
            )
    }

    guestLogin(): void {
        this._toolBarService.isLoading = true;
        this._authService.guestLogin()
            .subscribe(
                (authDto: AuthDto) => {
                    this._authService.token = authDto.token;
                    this._authService.user = authDto.userDto;
                    this._router.navigate(['/content/dashboard'])
                        .finally(() => this._toolBarService.isLoading = false); 
                },
                () => {
                    alert('Failed to login as guest');
                }
            )
    }
}