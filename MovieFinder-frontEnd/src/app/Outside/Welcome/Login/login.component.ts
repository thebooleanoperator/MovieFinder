import { Component } from '@angular/core';
import { SignInService } from '../../../Services/sign-in.service';
import { Router } from '@angular/router';
import { UserService } from '../../../Services/user.service';
import { AuthDto } from 'src/app/Dto/auth.dto';
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
        this.signInService.login(email, password);
    }
}