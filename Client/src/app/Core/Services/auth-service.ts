import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthDto } from '../../Data/auth.dto';
import { Router } from '@angular/router';
import { UserDto } from '../../Data/user.dto';

@Injectable({providedIn: 'root'})
export class AuthService {   
    constructor(private http: HttpClient, private router: Router)
    {

    }

    public register(firstName: string, lastName: string, email: string, password: string): Promise<void> {
        return this.http.post('http://localhost:5001/Accounts/Register', {"firstName": firstName, "lastName": lastName, "Email": email, "Password": password}).toPromise()
            .then(
                (response : AuthDto) => {
                    this.token = response.token;
                    this.user = response.userDto;
                    this.router.navigate(['/dashboard']); 
                }
            )
    }

    public login(email: string, password: string): Promise<void> {
        return this.http.post('http://localhost:5001/Accounts/Login', {"Email": email, "Password": password}).toPromise()
            .then(
                (response : AuthDto) => {
                    this.token = response.token;
                    this.user = response.userDto;
                    this.router.navigate(['/dashboard']); 
                }
            )
    }

    public logout(reRoute=true) {
        sessionStorage.clear();
        reRoute ? this.router.navigate(['/welcome']): null;
    }

    public isLoggedIn() : boolean {
        var currentUser = this.user;
        var currentToken = this.token;

        if (currentToken == null || currentUser == null) {
            return false
        }
        return true;
    }

    get token(): string {
        return sessionStorage.getItem('token')
    }

    set token(token : string) {
        sessionStorage.setItem('token', token);
    } 

    get user(): UserDto {
        return JSON.parse(sessionStorage.getItem('user'));
    }

    set user(user: UserDto) {
        sessionStorage.setItem('user', JSON.stringify(user))
    }
}