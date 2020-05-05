import { Component, OnInit } from '@angular/core';
import { ToolBarService } from '../../Services/tool-bar.service';
import { NavigationEnd, Router } from '@angular/router';
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
        this.navEnd = _router.events.pipe(
            filter(event => event instanceof NavigationEnd)
        ) as Observable<NavigationEnd>
    }
    
    navEnd: Observable<NavigationEnd>;
    searchUrl: string = "/dashboard";
    favoritesUrl: string = ""; 
    staffPickUrl: string = "/movies";
    isSearch: boolean;
    isFavorites: boolean;
    isStaffPicks: boolean; 

    ngOnInit(): void {
        this.navEnd.subscribe((event) => {
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

    setSelectedNavButton(search:boolean, favorites:boolean, staff:boolean): void {
        this.isSearch = search;
        this.isFavorites = favorites;
        this.isStaffPicks = staff;
    }
    
    getIsLoading(): boolean {
        return this._toolBarService.isLoading; 
    }

    openSettings(): void {
        this._settingsSheet.open(SettingsComponent)
    }
}
