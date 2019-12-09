import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({providedIn:'root'})
export class MoviesService {   
    constructor(private http: HttpClient){};

    public getMoviesByTitle(title: string): Observable<any> {
        return this.http.get(`http://localhost:5001/Movies/?=${title}`); 
    }      

    public getMovieGenre() {
        
    }
}