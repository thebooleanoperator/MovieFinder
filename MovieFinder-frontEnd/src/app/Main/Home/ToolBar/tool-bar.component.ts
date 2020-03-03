import { Component, OnInit } from '@angular/core';
import { SignInService } from '../../../Services/sign-in.service';

@Component({
  selector: 'tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: ['./tool-bar.component.scss']
})
export class ToolBarComponent{
    constructor(private signInService: SignInService)
    {

    }

    //Methods
    logout(): void {
        this.signInService.logout();
    }
}
