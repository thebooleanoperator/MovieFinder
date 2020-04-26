import { MatBottomSheetRef } from '@angular/material/bottom-sheet';
import { Component } from '@angular/core';

@Component({
    selector: 'settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss']
})
export class SettingsComponent {
    constructor(private _bottomSheetRef: MatBottomSheetRef<SettingsComponent>) {}

    openLink(event: MouseEvent): void {
        this._bottomSheetRef.dismiss();
        event.preventDefault();
    }
}