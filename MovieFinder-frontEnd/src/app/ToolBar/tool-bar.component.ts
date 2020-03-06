import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Services/auth-service';
import { ToolBarService } from '../Services/tool-bar.service';

@Component({
  selector: 'tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: ['./tool-bar.component.scss']
})
export class ToolBarComponent{
    constructor(private authService: AuthService, private toolBarService: ToolBarService)
    {

    }
    //Methods
    logout(): void {
        this.authService.logout();
    }

    getIsLoading(): boolean {
        return this.toolBarService.isLoading; 
    }
}
