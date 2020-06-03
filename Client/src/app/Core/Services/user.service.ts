import { Injectable } from "@angular/core";
import { UserDto } from '../../Data/Interfaces/user.dto'; 
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class UserService {
    public user : UserDto;
    constructor(private http : HttpClient)
    {

    }

    public getUser(userId : number) : Observable<any> {
        return this.http.get(`http://localhost:5001/Users/${userId}`);
    }
}