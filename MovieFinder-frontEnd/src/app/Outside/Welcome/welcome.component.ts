import { Component } from '@angular/core';

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