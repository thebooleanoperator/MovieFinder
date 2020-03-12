import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth-service';

@Component({
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent {
    //Data
    showLogin: boolean = true; 
    showRegister: boolean = false; 

    //Methods
    toggleLoginAndRegister() {
        this.showLogin = !this.showLogin;
        this.showRegister = !this.showRegister;
    }
}