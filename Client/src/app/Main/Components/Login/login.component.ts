import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../../Core/Services/auth-service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { Router } from '@angular/router';
import { AuthDto } from 'src/app/Data/Interfaces/auth.dto';
import { MatDialog } from '@angular/material';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent {
    constructor(
        private _authService: AuthService, 
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

    // Outputs
    @Output() toggleForm: EventEmitter<boolean> = new EventEmitter<boolean>();
    
    //Data
    loginForm: FormGroup; 
    isLoading: boolean;
    hide: boolean = true;

    //Methods
    verifyUserAndLogin(loginForm: FormGroup) {
        var email = loginForm.controls.userEmail.value; 
        var password = loginForm.controls.password.value;
        
        this.isLoading = true;
        this._authService.login(email, password)
            .subscribe(
                (authDto: AuthDto) => {
                    this._authService.token = authDto.token;
                    this._authService.user = authDto.userDto;
                    this._router.navigate(['/content/dashboard'])
                        .finally(() => this.isLoading = false);
                },
                (error) => {
                    alert(error.error);
                    this.isLoading = false;
                }
            )
    }

    showRegister(): void {
        this.toggleForm.emit(false);
    }

    guestLogin(): void {
        this.isLoading = true;
        this._authService.guestLogin()
            .subscribe(
                (authDto: AuthDto) => {
                    this._authService.token = authDto.token;
                    this._authService.user = authDto.userDto;
                    this._router.navigate(['/content/dashboard'])
                        .finally(() => this.isLoading = false); 
                },
                () => {
                    alert('Failed to login as guest');
                    this.isLoading = false;
                }
            )
    }
}