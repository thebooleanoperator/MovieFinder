import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd, NavigationStart, NavigationError, NavigationCancel } from '@angular/router';
import { AuthService } from './Core/Services/auth-service';
import { Observable } from 'rxjs';
import { ToolBarService } from './Core/Services/tool-bar.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
    constructor(
        private _router: Router, 
        private authService: AuthService, 
        private _toolBarService: ToolBarService){}

    //Data
    navEnd: Observable<NavigationEnd>;
    navStart: Observable<NavigationStart>;
    
    //Methods
    isLoggedOn(): boolean {
        return this.authService.isLoggedIn();
    }
    
    ngOnInit() {
        this._router.events.subscribe(
            (event) => {
                if (event instanceof NavigationStart) {
                    this._toolBarService.isLoading = true;
                }
                if (event instanceof NavigationEnd || event instanceof NavigationCancel || event instanceof NavigationError) {
                    // Log user out when navigate to outside page.
                    if (event.url == "/welcome") {
                        this.authService.logout(false);
                    }
                    this._toolBarService.isLoading = false;
                }
            }
        )
    }

    ngOnDestroy() {
        
    }
}
