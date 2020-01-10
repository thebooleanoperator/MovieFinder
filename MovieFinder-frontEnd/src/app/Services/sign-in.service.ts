import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({providedIn:'root'})
export class SignInService {   
    constructor(private http: HttpClient){};

    public register(firstName: string, lastName: string, email: string, password: string): Observable<Object> {
        return this
        .http
        .post('http://localhost:5001/Accounts/Register', {"firstName": firstName, "lastName": lastName, "Email": email, "Password": password}) 
    }

    public login(email: string, password: string): Observable<Object> {
        return this
        .http
        .post('http://localhost:5001/Accounts/Login', {"Email": email, "Password": password}) 
    }
}