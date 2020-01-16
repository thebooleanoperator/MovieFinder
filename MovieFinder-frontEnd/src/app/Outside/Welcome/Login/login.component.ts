import { Component } from '@angular/core';
import { SignInService } from '../../../Services/sign-in.service';
import { Router } from '@angular/router';
import { UserService } from '../../../Services/user.service';

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})

export class LoginComponent {
    constructor(private signInService: SignInService, private router: Router, private userService: UserService){}
    //Data
    email: string; 
    password: string; 

    //Methods
    verifyUserAndLogin(email, password) {
        this.signInService.login(email, password)
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