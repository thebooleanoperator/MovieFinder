import { Injectable } from "@angular/core";
import { UserDto } from '../../Data/Interfaces/user.dto'; 

@Injectable({providedIn: 'root'})
export class UserService {
    constructor(){}

    public getUser() : UserDto {
        return JSON.parse(localStorage.getItem('user'));
    }
}