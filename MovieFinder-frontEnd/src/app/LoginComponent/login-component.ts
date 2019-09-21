import { Component } from '@angular/core';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  title: string = 'Movie Finder TM';
  username: string;
  password: string;
  
  printMsg = function(username, password) {
    console.log(username, password); 
  }
}