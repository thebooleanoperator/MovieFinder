import { Component } from '@angular/core';
import { LoginService } from '../../../Services/login.service';
import { Router } from '@angular/router';
import { UserService } from '../../../Services/user.service';

@Component({
    selector: 'login',
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
                    /*var userDto = new UserDto(response); 
                    this.userService.setUser(userDto);
                    this.userService.user.next(userDto);*/
                    this.router.navigate(['dashboard']);
                }
                else {
                    console.log('failure');
                }
            }); 
    }
}