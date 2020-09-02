import { Component } from '@angular/core';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { AuthDto } from 'src/app/Data/Interfaces/auth.dto';
import { Router } from '@angular/router';
import { GuestHelpDialog } from '../../Dialogs/Guest-Help/guest-help-dialog';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
    constructor(
        private _authService: AuthService, 
        private _toolBarService: ToolBarService, 
        private _router: Router,
        private _formBuilder: FormBuilder,
        private _dialog: MatDialog)
        {
            this.registerForm = this._formBuilder.group
            (
                {
                    firstName: ['', [Validators.required]],
                    lastName: ['', [Validators.required]],
                    userEmail: ['', [Validators.required, Validators.email]],
                    password: ['', [Validators.required, Validators.minLength(7)]],
                }
            )
        }

    //Data
    registerForm: FormGroup;
    hide: boolean = true;
    isLoading: boolean;

    //Methods
    registerUser(registerForm: FormGroup): void {
        var firstName = registerForm.controls.firstName.value;
        var lastName = registerForm.controls.lastName.value;
        var email = registerForm.controls.userEmail.value;
        var password = registerForm.controls.password.value;
        this._toolBarService.isLoading = true;
        this.isLoading = true;
        this._authService.register(firstName, lastName, email, password)
            .subscribe(
                (authDto: AuthDto) => {
                    this._authService.token = authDto.token;
                    this._authService.user = authDto.userDto;
                    this._router.navigate(['/content/dashboard'])
                        .finally(() => this._toolBarService.isLoading = false); 
                },
                () => {
                    alert("Login failed");
                }
            );
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

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    openGuestHelpDialog() {
        this._dialog.open(GuestHelpDialog, {});
    }
}