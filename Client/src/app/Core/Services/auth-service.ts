import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest } from '@angular/common/http';
import { AuthDto } from '../../Data/Interfaces/auth.dto';
import { Router } from '@angular/router';
import { UserDto } from '../../Data/Interfaces/user.dto';
import { map } from 'rxjs/internal/operators/map';

@Injectable({providedIn: 'root'})
export class AuthService {   
    constructor(private http: HttpClient, private router: Router){}

    // Data
    failedRequestCache: Array<HttpRequest<any>>; 

    // Methods
    register(firstName: string, lastName: string, email: string, password: string): Promise<void> {
        return this.http.post('http://localhost:5001/Accounts/Register', {"firstName": firstName, "lastName": lastName, "Email": email, "Password": password}).toPromise()
            .then(
                (response : AuthDto) => {
                    this.token = response.token;
                    this.user = response.userDto;
                    this.router.navigate(['/dashboard']); 
                }
            )
    }

    login(email: string, password: string): Promise<void> {
        return this.http.post('http://localhost:5001/Accounts/Login', {"Email": email, "Password": password}).toPromise()
            .then(
                (response : AuthDto) => {
                    this.token = response.token;
                    this.user = response.userDto;
                    this.setRefreshToken(response.refreshToken);
                    this.router.navigate(['/dashboard']); 
                }
            )
    }

    
    refreshToken() {
        var jwtToken = this.token; 
        return this.http.post('http://localhost:5001/Accounts/RefreshToken', {'Token': jwtToken})
            .pipe(
                map((response: AuthDto) => {
                        this.token = response.token;
                        this.user = response.userDto;
                        this.setRefreshToken(response.refreshToken);
                        return response;
                    }
                )
            )
    }

    logout(reRoute=true) {
        sessionStorage.clear();
        reRoute ? this.router.navigate(['/welcome']): null;
    }

    isLoggedIn() : boolean {
        var currentUser = this.user;
        var currentToken = this.token;

        if (currentToken == null || currentUser == null) {
            return false
        }
        return true;
    }

    private setRefreshToken(token: string) {
        document.cookie = `refreshToken=${token}; path=/;`;
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