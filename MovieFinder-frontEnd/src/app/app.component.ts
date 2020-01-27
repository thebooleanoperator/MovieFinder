import { Component, OnInit } from '@angular/core';
import { NavigationStart, Router, NavigationEnd, Event } from '@angular/router';
import { ToolBarService } from './Services/tool-bar.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
    constructor(private toolBarService: ToolBarService, private router: Router)
    {
        this.router.events.subscribe((event: Event) => {
            if (event instanceof NavigationStart) {
                this.showProgressBar = true;
                this.showToolBar = this.toolBarService.vis;
            }
            if (event instanceof NavigationEnd) {
                this.showProgressBar = false;
            }
        });
    }
    //Data
    showProgressBar: boolean;
    showToolBar: boolean;

    //Methods
    ngOnInit() {
        this.showToolBar = this.toolBarService.vis;
    }
}
