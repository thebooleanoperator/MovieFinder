import { Component, OnInit } from '@angular/core';
import { SignInService } from './Services/sign-in.service';
import { NavigationStart, Router, NavigationEnd, Event } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
    constructor(private signInService: SignInService, private router: Router)
    {
        this.router.events.subscribe((event: Event) => {
            if (event instanceof NavigationStart) {
                this.showProgressBar = true;
            }
            if (event instanceof NavigationStart && event.url == "/") {
                this.showToolbar = false;
            }
            if (event instanceof NavigationEnd) {
                this.showProgressBar = false;
            }
        });
    }
    //Data
    showToolbar: boolean = this.signInService.isLoggedIn();
    showProgressBar: boolean;

    //Methods
    ngOnInit() {
        this.showToolbar = this.signInService.isLoggedIn();
    }
}
