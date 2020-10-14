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
        private _formBuilder: FormBuilder) {
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

    /**
     * Make sure new password and confirm password match or trigger mismatch form error.
     * @param changePasswordGroup 
     */
    matchValues(changePasswordGroup: FormGroup) {
        var newPassword = changePasswordGroup.controls.newPassword.value;
        var confirmPassword = changePasswordGroup.controls.confirmPassword.value;

        return newPassword === confirmPassword ? null : {mismatch: true};
    }

    /**
     * Change user password then either alert error or show success dialog.
     * @param changePasswordGroup
     */
    updatePassword(changePasswordGroup: FormGroup) {
        var changePassword = this.buildChangePassword(changePasswordGroup);
        this._toolBarService.isLoading = true;
        this.isUpdating = true;
        this._authService.updatePassword(changePassword)
            .subscribe(
                () => {
                    this.showUpdateSuccess();
                },
                (error) => {
                    alert(error.error);
                    this._toolBarService.isLoading = false;
                    this.isUpdating = false;
                },
                () => {
                    this._toolBarService.isLoading = false;
                    this.isUpdating = false;
                }
            )
    }

    /**
     * Builds a new ChangePassword object from FormGroup data.
     * @param changePasswordGroup
     */
    buildChangePassword(changePasswordGroup: FormGroup): ChangePassword {
        var email = this._authService.user.email;
        var oldPassword = changePasswordGroup.controls.oldPassword.value;
        var newPassword = changePasswordGroup.controls.newPassword.value;
        var confirmPassword = changePasswordGroup.controls.confirmPassword.value;
        return new ChangePassword(email, oldPassword, newPassword, confirmPassword);
    }

    /**
     * Show success modal for 1 second then close.
     */
    showUpdateSuccess() {
        this.showInputs = false;
        this.showSuccess = true;
        window.setTimeout(() => {
            this._dialogRef.close();
        }, 1000)
    }
}   
