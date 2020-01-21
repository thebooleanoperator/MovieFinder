import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthDto } from '../Dto/auth.dto';
import { Router } from '@angular/router';

@Injectable({providedIn:'root'})
export class SignInService {   
    constructor(private http: HttpClient, private router: Router){};

    public register(firstName: string, lastName: string, email: string, password: string): Promise<Object> {
        return this.http.post('http://localhost:5001/Accounts/Register', {"firstName": firstName, "lastName": lastName, "Email": email, "Password": password}).toPromise()
            .then(
                (response : AuthDto) => {
                    return response;
                },
                (error) => {
                    return error
                }
            )
    }

    public login(email: string, password: string): Promise<Object> {
        return this.http.post('http://localhost:5001/Accounts/Login', {"Email": email, "Password": password}).toPromise()
            .then(
                (response : AuthDto) => {
                    this.setToken(response.token);
                    this.router.navigate(['/dashboard']); 
                },
                (error) => {
                    return error;
                }
            )
    }

    public logout() {
        this.resetToken();
        this.router.navigate(['/welcome']);
    }

    private setToken(token : string) : void{
        localStorage.setItem('token', token);
    }

    public getToken() : string {
        return localStorage.getItem('token')
    }

    private resetToken() : void{
        localStorage.clear();
        
    }
}