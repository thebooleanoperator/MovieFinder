import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ImdbIdDto } from '../../Data/imdbId.dto';

@Injectable({providedIn:'root'})
export class MoviesService {   
    constructor(private http: HttpClient){};
    
    public getRecommended(): Observable<any> {
        return this.http.get('http://localhost:5001/Movies/Recommended');
    } 
    
    public getFavorites(page:number, count:number): Observable<any> {
        return this.http.get(`http://localhost:5001/Movies/Favorites?page=${page}&count=${count}`);
    }

    public getMovieByImdbId(imdbId: string): Observable<any> {
        return this.http.get(`http://localhost:5001/Movies/${imdbId}`);  
    }

    public createMovieFromImdbId(imdbIdDto: ImdbIdDto): Observable<any> {
        return this.http.post('http://localhost:5001/Movies', imdbIdDto);  
    }
}