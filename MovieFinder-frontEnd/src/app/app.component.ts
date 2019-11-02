import { Component, OnInit } from '@angular/core';
import { UserService } from './Services/user.service';
import { UserDto } from './DTO/user.dto'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit{
    constructor(private userService: UserService){}

    loggedInUser; 

    ngOnInit() {
        this.userService.user.subscribe((user) => {
            this.loggedInUser = user; 
        })
    }
}
