import { Component, OnInit } from '@angular/core';
import { NavigationStart, Router, NavigationEnd, Event } from '@angular/router';
import { AuthService } from './Services/auth-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
    constructor(private authService: AuthService)
    {
    }
    
    //Methods
    isLoggedOn(): boolean {
        return this.authService.isLoggedIn();
    }
}
