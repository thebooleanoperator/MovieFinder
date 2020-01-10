import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../Services/user.service';
import { SignInService } from 'src/app/Services/sign-in.service';

@Component({
  selector: 'register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
    firstName: string;
    lastName: string;
    email: string;
    password: string;

    constructor(private signInService: SignInService, private router: Router, private userService: UserService){}

    registerUser(firstName, lastName, email, password) {
        this.signInService.register(firstName, lastName, email, password)
            .subscribe((response) => {
                if (response) {
                    this.router.navigate(['dashboard']);
                }
                else {
                    console.log('failure');
                }
            })
    }
}