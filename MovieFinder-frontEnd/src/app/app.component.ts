import { Component, OnInit, HostListener } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from './Services/auth-service';
import { Observable } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    /*wip
    * logoutTimeout: NodeJS.Timer;
    @HostListener('window:beforeunload')
    handleUnload() {
        this.logoutTimeout = setTimeout(() => {
            return this.authService.logout(false);
        }, 10000);
    } 

    @HostListener('window:onloadon')
    handleLoad() {  
        if (this.logoutTimeout) {
            clearTimeout(this.logoutTimeout); 
        }
    }*/

    constructor(private authService: AuthService, private router: Router)
    {
        // Returns a NavigationEnd observable, so we can check the url on route change end.
        this.navEnd = router.events.pipe(
            filter(event => event instanceof NavigationEnd)
        ) as Observable<NavigationEnd>
    }

    //Data
    navEnd: Observable<NavigationEnd>;

    //Methods
    isLoggedOn(): boolean {
        return this.authService.isLoggedIn();
    }
    
    ngOnInit() {
        // Log the user out, and remove session when a user navigates to outside pages.
        this.navEnd.subscribe((event) => {
            if (event.url == "/welcome") {
                this.authService.logout(false);
            }
        })
    }
}
