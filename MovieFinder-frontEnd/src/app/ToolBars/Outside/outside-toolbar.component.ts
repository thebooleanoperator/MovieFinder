import { Component } from "@angular/core";

@Component({
    selector: 'outside-toolbar',
    templateUrl: './outside-toolbar.component.html',
    styleUrls: ['./outside-toolbar.component.scss']
})
export class OutsideToolbarComponent {
    //Data
    showLogin: boolean = true;
    showRegister: boolean = true;
    showWelcome: boolean = false;

    //Methods
    toggleButtons(login:boolean, register:boolean, welcome:boolean): void {
        this.showLogin = login;
        this.showRegister = register;
        this.showWelcome = welcome;
    }
}