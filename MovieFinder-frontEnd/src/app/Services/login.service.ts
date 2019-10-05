import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({providedIn: 'root'})
export class LoginService {   
    path = 'http://localhost:5001/Users/Login'; 
    loggedIn: boolean = false; 

    constructor(private http: HttpClient){};

    userLoggedIn(): boolean {
        return this.loggedIn;
    }

    loginSucess(): void {
        this.loggedIn = true; 
    }

    validateLogin(email: string, password: string): Observable<Object> {
        return this
        .http
        .post(this.path, {"Email": email, "Password": password})  
    }          
}