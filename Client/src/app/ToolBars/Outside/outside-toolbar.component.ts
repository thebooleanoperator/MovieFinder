import { Component, Input } from "@angular/core";

@Component({
    selector: 'outside-toolbar',
    templateUrl: './outside-toolbar.component.html',
    styleUrls: ['./outside-toolbar.component.scss']
})
export class OutsideToolbarComponent {
    @Input() showWelcome: boolean;
    @Input() showLogin: boolean;
    @Input() showRegister: boolean;
}