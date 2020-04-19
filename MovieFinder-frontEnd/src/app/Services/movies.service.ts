import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ImdbIdDto } from '../Dto/imdbId.dto';

@Injectable({providedIn:'root'})
export class MoviesService {   
    constructor(private http: HttpClient){};
    
    public getRecommended(): Observable<any> {
        return this.http.get('http://localhost:5001/Movies/Recommended');
    } 

    public getMovieByImdbId(imdbId: string): Observable<any> {
        return this.http.get(`http://localhost:5001/Movies/${imdbId}`);  
    }

    public createMovieFromImdbId(imdbIdDto: ImdbIdDto): Observable<any> {
        return this.http.post('http://localhost:5001/Movies', imdbIdDto);  
    }
}