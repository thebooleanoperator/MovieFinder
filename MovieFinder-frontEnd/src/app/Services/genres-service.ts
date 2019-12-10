import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({providedIn:'root'})
export class GenresService {   
    constructor(private http: HttpClient){};

    public getGenresFromMovieId(movieId: number): Observable<any> {
        return this.http.get(`http://localhost:5001/Genres/${movieId}`); 
    }      
}