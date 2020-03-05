import { Component, OnInit } from '@angular/core';
import { NavigationStart, Router, NavigationEnd, Event } from '@angular/router';
import { SignInService } from './Services/sign-in.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
    constructor(private signInService: SignInService)
    {
    }
    
    //Methods
    isLoggedOn(): boolean {
        return this.signInService.isLoggedIn();
    }
}
