import { Injectable } from "@angular/core";
import { Router } from '@angular/router';
import { UserDto } from '../Dto/user.dto'; 
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class UserService {
    public user : UserDto;
    constructor(private http : HttpClient)
    {

    }

    public getUser() : Observable<any> {
        var userId = parseInt(localStorage.getItem('userId'), 10); 
        return this.http.get(`http://localhost:5001/Users/${userId}`);
    }
}