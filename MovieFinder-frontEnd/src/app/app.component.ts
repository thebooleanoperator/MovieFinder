import { Component, OnInit } from '@angular/core';
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
    constructor(private authService: AuthService, private router: Router)
    {
        // Returns a NavigationEnd observable, so we can check the url on route change end.
        this.navEnd = router.events.pipe(
            filter(event => event instanceof NavigationEnd)
        ) as Observable<NavigationEnd>
    }

    //Data
    navEnd: Observable<NavigationEnd>;
    outsideUrls: Array<string> = ["/welcome", "/login", "/register"];
    showLogin: boolean = true;
    showRegister: boolean = true;
    showWelcome: boolean = false;
    
    //Methods
    isLoggedOn(): boolean {
        return this.authService.isLoggedIn();
    }
    
    ngOnInit() {
        // Log the user out, and remove session when a user navigates to outside pages.
        this.navEnd.subscribe((event) => {
            if (this.outsideUrls.includes(event.url)) {
                this.authService.logout(false);
            }
        })
    }
}
