import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({providedIn:'root'})
export class RegisterService {   
    path = 'http://localhost:5001/Accounts/Register'; 

    constructor(private http: HttpClient){};

    public register(firstName: string, lastName: string, email: string, password: string): Observable<Object> {
        return this
        .http
        .post(this.path, {"firstName": firstName, "lastName": lastName, "Email": email, "Password": password}) 
    }
}