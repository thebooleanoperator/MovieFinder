import { Component, Input, OnInit } from '@angular/core';
import { AuthService } from '../../Services/auth-service';
import { ToolBarService } from '../../Services/tool-bar.service';
import { NavigationEnd, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/internal/operators/filter';

@Component({
  selector: 'inside-toolbar',
  templateUrl: './inside-toolbar.component.html',
  styleUrls: ['./inside-toolbar.component.scss']
})
export class InsideToolbarComponent implements OnInit{
    constructor(private authService: AuthService, private toolBarService: ToolBarService, private router: Router)
    {
        // Returns a NavigationEnd observable, so we can check the url on route change end.
        this.navEnd = router.events.pipe(
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
    logout(): void {
        this.authService.logout();
    }

    getIsLoading(): boolean {
        return this.toolBarService.isLoading; 
    }
}
