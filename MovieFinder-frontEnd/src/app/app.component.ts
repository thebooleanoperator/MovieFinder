import { Component, OnInit } from '@angular/core';
import { UserService } from './Services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
    loggedInUser: Object = JSON.parse(localStorage.getItem("userInfo"));
    constructor(private userService: UserService){}

    ngOnInit() {
        this.userService.user.subscribe(() => {
            this.loggedInUser = JSON.parse(localStorage.getItem("userInfo")); 
        })
    }
}
