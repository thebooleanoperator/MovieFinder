import { Component, OnInit } from '@angular/core';
import { ToolBarService } from '../../Services/tool-bar.service';
import { NavigationEnd, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/internal/operators/filter';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { SettingsComponent } from 'src/app/Main/Home/Settings/settings.component';

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

    //Data
    navEnd: Observable<NavigationEnd>;
    searchUrl: string = "/dashboard"; 
    staffPickUrl: string = "/movies"; 
    isSearch: boolean = true; 

    ngOnInit(): void {
        this.navEnd.subscribe((event) => {
            switch(event.url) {
                case this.searchUrl:
                    this.isSearch = true;
                    break;
                case this.staffPickUrl:
                    this.isSearch = false;
                    break;
            }
        })
    }
    
    //Methods
    getIsLoading(): boolean {
        return this._toolBarService.isLoading; 
    }

    openSettings(): void {
        this._settingsSheet.open(SettingsComponent)
    }
}
