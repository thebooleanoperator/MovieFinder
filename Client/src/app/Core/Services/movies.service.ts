import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ImdbIdDto } from '../../Data/Interfaces/imdbId.dto';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { map, concatMap } from 'rxjs/operators';
import { of } from 'rxjs'

@Injectable({providedIn:'root'})
export class MoviesService {   
    constructor(private http: HttpClient){};
    
    public getRecommended(): Observable<any> {
        return this.http.get('http://localhost:5001/Movies/Recommended');
    } 
    
    public getFavorites(skip:number, count:number): Observable<any> {
        return this.http.get(`http://localhost:5001/Movies/Favorites?skip=${skip}&count=${count}`);
    }

    public getMovieByImdbId(imdbId: string): Observable<any> {
        return this.http.get(`http://localhost:5001/Movies/${imdbId}`);
    }

    public createMovieFromImdbId(imdbIdDto: ImdbIdDto): Observable<any> {
        return this.http.post('http://localhost:5001/Movies', imdbIdDto);
    }

    public getMovieSearchHistory(historyLength: number =null): Observable<any> {
        return this.http.get(`http://localhost:5001/Movies/SearchHistory?historyLength=${historyLength}`);
    }

    public $getOrCreateMovie(imdbId: string, imdbIdDto:ImdbIdDto): Observable<any> {
        return this.getMovieByImdbId(imdbId)
            .pipe (
                concatMap((movieDto: MovieDto) => {
                    if (movieDto) {
                        return of(movieDto);
                    }
                    return this.createMovieFromImdbId(imdbIdDto)
                        .pipe (
                            map((movieDto: MovieDto) => {
                                return movieDto
                            })
                        )
                })
            )
    }
}