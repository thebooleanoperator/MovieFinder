import { MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { ChangePasswordDialog } from '../../Dialogs/ResetPassword/change-password-dialog';
import { MatDialog } from '@angular/material/dialog';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';

@Component({
    selector: 'settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss']
})
export class SettingsComponent {
    constructor(
        private _bottomSheetRef: MatBottomSheetRef<SettingsComponent>, 
        private _authService: AuthService,
        private _toolBarService: ToolBarService,
        private dialog: MatDialog) {}
    
    logout() {
        this._authService.logout();
        this._bottomSheetRef.dismiss();
        event.preventDefault();
    }

    /**
     * Opens the angular material dialogRef and passes the resetPassword to the dialog.
     */
    openResetPasswordDialog() {
        this.dialog.open(ChangePasswordDialog, {
            id: 'resetPwDialog'
        });
    }
}