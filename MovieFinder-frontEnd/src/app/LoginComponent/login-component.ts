import { Component } from '@angular/core';
import { LoginService } from './login.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [LoginService]
})

export class LoginComponent {
  title: string = 'Movie Finder TM';
  email: string; 
  password: string; 

  constructor(private LoginService: LoginService){};

  verifyLogin(email, password): void {
    this.LoginService.validateLogin(email, password)
      .subscribe((response) => {
        console.log(response); 
      })
    }
}