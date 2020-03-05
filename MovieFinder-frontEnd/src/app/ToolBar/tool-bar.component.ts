import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/auth-service';

@Component({
  selector: 'tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: ['./tool-bar.component.scss']
})
export class ToolBarComponent{
    constructor(private authService: AuthService)
    {

    }

    //Methods
    logout(): void {
        this.authService.logout();
    }
}
