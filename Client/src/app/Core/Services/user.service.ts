import { Injectable } from "@angular/core";
import { UserDto } from '../../Data/Interfaces/user.dto'; 
import { HttpClient } from '@angular/common/http';

@Injectable({providedIn: 'root'})
export class UserService {
    public user : UserDto;
    constructor(private http : HttpClient)
    {

    }

    public getUser(userId : number) : Promise<Object> {
        return this.http.get(`http://localhost:5001/Users/${userId}`).toPromise();
    }
}