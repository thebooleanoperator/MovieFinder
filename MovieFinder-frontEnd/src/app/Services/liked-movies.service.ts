import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({providedIn:'root'})
export class LikedMoviesService {   
    constructor(private http: HttpClient){};

    public getLikedMovies(): Observable<any> {
        return this.http.get(`http://localhost:5001/LikedMovies`) 
    }          
}