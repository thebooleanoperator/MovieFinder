import { Component } from '@angular/core';
import { LoginService } from '../../Services/login.service';
import { Router } from '@angular/router';
import { UserService } from '../../Services/user.service';
import { UserDto } from '../../DTO/user.dto';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
    providers: [LoginService]
})

export class LoginComponent {
    email: string; 
    password: string; 
    
    constructor(private loginService: LoginService, private router: Router, private userService: UserService){}


    verifyUserAndLogin(email, password) {
        this.loginService.login(email, password)
            .subscribe((response) => {
                if (response) {
                    var userDto = new UserDto(response); 
                    this.userService.setUser(userDto);
                    this.userService.user.next(userDto);
                    this.router.navigate(['home']);
                }
                else {
                    console.log('failure');
                }
            }); 
    }
}