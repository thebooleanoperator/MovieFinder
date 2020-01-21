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
    constructor(private signInService: SignInService, private router: Router, private userService: UserService){}
    //Data
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    isLoading: boolean = false;

    //Methods
    registerUser(firstName, lastName, email, password) {
        this.isLoading = true;
        this.signInService.register(firstName, lastName, email, password)
            .finally(() => this.isLoading = false);
    }
}