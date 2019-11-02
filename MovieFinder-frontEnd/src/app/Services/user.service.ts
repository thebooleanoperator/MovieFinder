import { Injectable } from '@angular/core';
import { UserDto } from '../DTO/user.dto';

@Injectable({providedIn:'root'})
export class UserService {   
    user: UserDto; 
    constructor(){};

    public setUser(user: UserDto) : void {
        this.user = user; 
    }
}