import { Component } from '@angular/core';
import { AuthService } from '../../Services/auth-service';
import { ToolBarService } from '../../Services/tool-bar.service';

@Component({
  selector: 'inside-toolbar',
  templateUrl: './inside-toolbar.component.html',
  styleUrls: ['./inside-toolbar.component.scss']
})
export class InsideToolbarComponent{
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
