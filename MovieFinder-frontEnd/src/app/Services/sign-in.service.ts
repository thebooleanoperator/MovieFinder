import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthDto } from '../Dto/auth.dto';
import { Router } from '@angular/router';
import { UserService } from './user.service';
import { UserDto } from '../Dto/user.dto';
import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class SignInService {   
    constructor(private http: HttpClient, private router: Router, private userService : UserService){};

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
                    this.saveToken(response.token);
                    this.saveUser(response.userId);
                    this.router.navigate(['/dashboard']); 
                },
                (error) => {
                    alert("Username / Password did not match")
                    return error;
                }
            )
    }

    public logout() {
        this.resetToken();
        this.router.navigate(['/welcome']);
    }

    public getToken(): string {
        return localStorage.getItem('token')
    }

    public isLoggedIn() : boolean {
        var token = this.getToken();
        if (token) {
            return true;
        }
        else {
            return false;
        }
    }

    private saveToken(token : string) : void{
        localStorage.setItem('token', token);
    }

    private saveUser(userId : number) {
        localStorage.setItem('userId', JSON.stringify(userId))
    }

    private resetToken() : void{
        localStorage.clear();
    }
}