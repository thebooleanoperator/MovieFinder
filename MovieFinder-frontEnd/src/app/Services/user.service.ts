import { Injectable } from '@angular/core';
import { UserDto } from '../DTO/user.dto';
import { Subject } from 'rxjs';

@Injectable({providedIn:'root'})
export class UserService {   
    user = new Subject();  
}