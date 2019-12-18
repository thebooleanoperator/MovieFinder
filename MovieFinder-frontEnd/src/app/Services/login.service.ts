import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({providedIn:'root'})
export class LoginService {   
    path = 'http://localhost:5001/Accounts/Login'; 

    constructor(private http: HttpClient){};

    public login(email: string, password: string): Observable<Object> {
        return this
        .http
        .post(this.path, {"Email": email, "Password": password}) 
    }
}