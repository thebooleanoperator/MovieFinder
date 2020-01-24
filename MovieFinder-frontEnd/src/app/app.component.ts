import { Component, OnInit } from '@angular/core';
import { SignInService } from './Services/sign-in.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
    constructor(private signInService: SignInService){}
    //Data
    loggedInUser: boolean = this.signInService.isLoggedIn();

    //Methods
    ngOnInit() {
        this.loggedInUser = this.signInService.isLoggedIn();
    }
}
