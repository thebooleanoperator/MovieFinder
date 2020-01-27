import { Component, OnInit } from '@angular/core';
import { ToolBarService } from '../Services/tool-bar.service';
import { SignInService } from '../Services/sign-in.service';
import { Router } from '@angular/router';

@Component({
  selector: 'tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: []
})
export class ToolBarComponent{
    constructor(private signInService: SignInService, private router: Router)
    {

    }

    //Methods
    logout(): void {
        this.signInService.logout();
    }
}
