import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { SettingsComponent } from 'src/app/Main/Components/Settings/settings.component';
import { ToolBarService } from '../../Services/tool-bar.service';

@Component({
  selector: 'inside-toolbar',
  templateUrl: './inside-toolbar.component.html',
  styleUrls: ['./inside-toolbar.component.scss']
})
export class InsideToolbarComponent {
    constructor(
        protected _router: Router, 
        private _settingsSheet: MatBottomSheet,
        private _toolBarService: ToolBarService)
    {

    }

    // Methods
    /**
     * Opens settings component. 
     */
    openSettings(): void {
        this._settingsSheet.open(SettingsComponent)
    }

    /**
     * Returns if the toolbar loading bar should show.
     */
    getIsLoading(): boolean {
        return this._toolBarService.isLoading; 
    }
}
