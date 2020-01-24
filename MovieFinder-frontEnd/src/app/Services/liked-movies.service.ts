import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { MovieDto } from '../DTO/movie.dto';

@Injectable({providedIn:'root'})
export class LikedMoviesService {   
    constructor(private http: HttpClient){};

    public getLikedMovies(): Promise<Object> {
        return this.http.get(`http://localhost:5001/LikedMovies`).toPromise(); 
    }          
}