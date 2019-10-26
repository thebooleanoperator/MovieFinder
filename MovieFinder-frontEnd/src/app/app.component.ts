import { Component } from '@angular/core';
import { LoginService } from './Services/login.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
    
    constructor(private LoginService: LoginService){}

    get loggedIn(): boolean {
        return this.LoginService.loggedIn;
    }
}
