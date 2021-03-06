import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { AuthDto } from '../../Data/Interfaces/auth.dto';
import { Router } from '@angular/router';
import { UserDto } from '../../Data/Interfaces/user.dto';
import { map } from 'rxjs/internal/operators/map';
import { Observable } from 'rxjs';
import { ChangePassword } from 'src/app/Data/Interfaces/change-password.dto';

@Injectable({providedIn: 'root'})
export class AuthService {   
    constructor(private http: HttpClient, private router: Router){}

    // Data
    failedRequestCache: Array<HttpRequest<any>>; 

    // Methods
    register(firstName: string, lastName: string, email: string, password: string): Observable<any> {
        return this.http.post('/Accounts/Register', {"firstName": firstName, "lastName": lastName, "Email": email, "Password": password});
    }

    login(email: string, password: string): Observable<any> {
        return this.http.post('/Accounts/Login', {"Email": email, "Password": password});
    }

    guestLogin(): Observable<any> {
        return this.http.post('/Accounts/GuestLogin', {}); 
    }

    refreshToken(): Observable<any> {
        return this.http.post(`/Accounts/RefreshToken/${this.user.userId}`, {})
            .pipe(
                map((response: AuthDto) => {
                        this.token = response.token;
                        this.user = response.userDto;
                        return response;
                    }
                )
            )
    }

    updatePassword(changePassword: ChangePassword): Observable<any> {
        return this.http.put('/Accounts/ChangePassword', changePassword)
            .pipe(
                map((response: AuthDto) => {
                    return response;
                })
            )
    }

    logout(reRoute=true) {
        localStorage.clear();
        reRoute ? this.router.navigate(['/welcome']): null;
    }

    isLoggedIn() : boolean {
        if (this.user == null || this.token == null) {
            return false
        }
        return true;
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
}