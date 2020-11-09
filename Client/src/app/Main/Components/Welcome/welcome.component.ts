import { Component } from '@angular/core';

@Component({
    selector: 'welcome',
    templateUrl: './welcome.component.html',
    styleUrls: ['./welcome.component.scss']
})
export class WelcomeComponent {
    showLogin: boolean = true;

    toggleForm($event): void {
        this.showLogin = $event;
    }
}