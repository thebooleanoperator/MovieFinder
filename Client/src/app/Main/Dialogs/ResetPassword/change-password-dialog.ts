import { Component } from "@angular/core";
import { MatDialogRef } from '@angular/material/dialog';
import { Validators, FormGroup, FormBuilder } from '@angular/forms';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { ChangePassword } from 'src/app/Data/Interfaces/change-password.dto';
import { FormErrorStateMatcher } from '../../FormErrorStateMatcher/form-error-state-matcher';


@Component({
    selector: 'change-password-dialog',
    templateUrl: './change-password-dialog.html',
    styleUrls: ['./change-password-dialog.scss']
})
export class ChangePasswordDialog {
    constructor(
        private _dialogRef: MatDialogRef<ChangePasswordDialog>,
        private _authService: AuthService,
        private _toolBarService: ToolBarService,
        private _formBuilder: FormBuilder){
            this.changePasswordForm = this._formBuilder.group
            (
                {
                    oldPassword: ['', [Validators.required]],
                    newPassword: ['', [Validators.required, Validators.minLength(7)]],
                    confirmPassword: ['']
                }, 
                {
                    validator: this.matchValues
                }
            )
        }

    changePasswordForm: FormGroup; 
    isUpdating: boolean;
    matcher = new FormErrorStateMatcher();
    hidePw: boolean = true;
    showInputs: boolean = true;
    showSuccess: boolean = false;

    matchValues(changePasswordGroup: FormGroup) {
        var newPassword = changePasswordGroup.controls.newPassword.value;
        var confirmPassword = changePasswordGroup.controls.confirmPassword.value;

        return newPassword === confirmPassword ? null : {mismatch: true};
    }

    /**
     * 
     * @param changePasswordGroup
     */
    updatePassword(changePasswordGroup: FormGroup) {
        var changePassword = this.buildChangePassword(changePasswordGroup);
        this._toolBarService.isLoading = true;
        this.isUpdating = true;
        this._authService.updatePassword(changePassword)
            .subscribe(
                () => {},
                ((error) => {
                    if (error.status != 401) {
                        alert("Failed to update password.");
                    }
                    this.isUpdating = false;
                    this._toolBarService.isLoading = false;
                }),
                () => {
                    this.isUpdating = false;
                    this._toolBarService.isLoading = false;
                    this.showUpdateSuccess();
                }
            )
    }

    /**
     * 
     * @param changePasswordGroup
     */
    buildChangePassword(changePasswordGroup: FormGroup): ChangePassword {
        var jwtToken = this._authService.token;
        var email = this._authService.user.email;
        var oldPassword = changePasswordGroup.controls.oldPassword.value;
        var newPassword = changePasswordGroup.controls.newPassword.value;
        var confirmPassword = changePasswordGroup.controls.confirmPassword.value;
        return new ChangePassword(email, jwtToken, oldPassword, newPassword, confirmPassword);
    }

    showUpdateSuccess() {
        this.showInputs = false;
        this.showSuccess = true;
        window.setTimeout(() => {
            this._dialogRef.close();
        }, 800)
    }
}   