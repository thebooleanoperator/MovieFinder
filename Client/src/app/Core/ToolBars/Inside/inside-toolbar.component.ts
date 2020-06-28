import { Component, OnInit } from '@angular/core';
import { ToolBarService } from '../../Services/tool-bar.service';
import { Router, ResolveEnd } from '@angular/router';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/internal/operators/filter';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { SettingsComponent } from 'src/app/Main/Components/Settings/settings.component';

@Component({
  selector: 'inside-toolbar',
  templateUrl: './inside-toolbar.component.html',
  styleUrls: ['./inside-toolbar.component.scss']
})
export class InsideToolbarComponent implements OnInit{
    constructor(private _toolBarService: ToolBarService, protected _router: Router, private _settingsSheet: MatBottomSheet)
    {
        // Returns a NavigationEnd observable, so we can check the url on route change end.
        this.resolveEnd = _router.events.pipe(
            filter(event => event instanceof ResolveEnd)
        ) as Observable<ResolveEnd>
    }
    
    resolveEnd: Observable<ResolveEnd>;
    searchUrl: string = "/dashboard";
    favoritesUrl: string = "/favorites"; 
    staffPickUrl: string = "/movies";
    navInProcess: boolean;
    isSearch: boolean = true;
    isFavorites: boolean;
    isStaffPicks: boolean; 

    ngOnInit() {
        this.resolveEnd.subscribe((event) => {
            switch(event.url) {
                case this.searchUrl:
                    this.setSelectedNavButton(true, false, false); 
                    break;
                case this.favoritesUrl:
                    this.setSelectedNavButton(false, true, false); 
                    break;
                case this.staffPickUrl:
                    this.setSelectedNavButton(false, false, true);
                    break;
            }
        })
    }

    /**
     * Returns if the toolbar loading bar should show.
     */
    getIsLoading(): boolean {
        return this._toolBarService.isLoading; 
    }

    /**
     * Opens settings component. 
     */
    openSettings(): void {
        this._settingsSheet.open(SettingsComponent)
    }

    /**
     * Navigates to dashboard. 
     */
    goToDashboard(): void {
        this.navInProcess = true;
        this._router.navigateByUrl(this.searchUrl)
            .then(() => this.setSelectedNavButton(true, false, false))
            .finally(() => this.navInProcess = false);
    }

    /**
     * Navigates to favorites.
     */
    goToFavorites(): void {
        this.navInProcess = true;
        this._router.navigateByUrl(this.favoritesUrl)
            .then(() => this.setSelectedNavButton(false, true, false))
            .finally(() => this.navInProcess = false);
    }

    /**
     * Navigates to recommended.
     */
    goToRecommended(): void {
        this.navInProcess = true;
        this._router.navigateByUrl(this.staffPickUrl)
            .then(() => this.setSelectedNavButton(false, false, true))
            .finally(() => this.navInProcess = false);
    }

    /**
     * Sets the booleans that toggle active classes for each button.
     * @param search 
     * @param favorites 
     * @param staff 
     */
    setSelectedNavButton(search:boolean, favorites:boolean, staff:boolean): void {
        this.isSearch = search;
        this.isFavorites = favorites;
        this.isStaffPicks = staff;
    }
}
