import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthDto } from '../Dto/auth.dto';
import { Router } from '@angular/router';
import { UserDto } from '../Dto/user.dto';

@Injectable({providedIn: 'root'})
export class SignInService {   
    constructor(private http: HttpClient, private router: Router){};

    public register(firstName: string, lastName: string, email: string, password: string): Promise<Object> {
        return this.http.post('http://localhost:5001/Accounts/Register', {"firstName": firstName, "lastName": lastName, "Email": email, "Password": password}).toPromise()
            .then(
                (response : AuthDto) => {
                    this.token = response.token;
                    this.user = response.userDto;
                    this.router.navigate(['/dashboard']); 
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
                    this.token = response.token;
                    this.user = response.userDto;
                    this.router.navigate(['/dashboard']); 
                },
                (error) => {
                    alert(error)
                    return error;
                }
            )
    }

    public logout() {
        localStorage.clear();;
        this.router.navigate(['/welcome']);
    }

    get token(): string {
        return localStorage.getItem('token')
    }

    set token(token : string) {
        localStorage.setItem('token', token);
    } 

    get user(): UserDto {
        return JSON.parse(localStorage.getItem('user'));
    }

    set user(user: UserDto) {
        localStorage.setItem('user', JSON.stringify(user))
    }

    public isLoggedIn() : boolean {
        var currentUser = this.user;
        var currentToken = this.token;

        if (currentToken == null || currentUser == null) {
            return false
        }
        return true;
    }
}