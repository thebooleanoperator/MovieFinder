import { MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { Component } from '@angular/core';
import { AuthService } from 'src/app/Core/Services/auth-service';

@Component({
    selector: 'settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss']
})
export class SettingsComponent {
    constructor(private _bottomSheetRef: MatBottomSheetRef<SettingsComponent>, private _authService: AuthService) {}
    
    logout() {
        this._authService.logout();
        this._bottomSheetRef.dismiss();
        event.preventDefault();
    }
    
}