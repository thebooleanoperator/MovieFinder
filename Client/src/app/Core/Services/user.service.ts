import { Injectable } from "@angular/core";
import { UserDto } from '../../Data/Interfaces/user.dto'; 
import * as jwt_decode from 'jwt-decode';
import { AuthService } from './auth-service';

@Injectable({providedIn: 'root'})
export class UserService {
    constructor(private _authService: AuthService){}

    public getUser() : UserDto {
        return JSON.parse(localStorage.getItem('user'));
    }

    /**
     * Parse jwtToken and pull userId out of it. If userId is 0 user is guest.
     */
    isGuest() : boolean {
        var token = this._authService.token; 
        try {
            var decodedToken = jwt_decode(token);
            var userId = parseInt(decodedToken['UserId']); 
        }
        catch(error) {
            console.log('Error: ' + error)
        }
        return userId == 0;
    }
}