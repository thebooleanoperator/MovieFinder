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

    public get(movieId: number): Observable<any> {
        return this.http.get(`/Movies/${movieId}`);
    }
    
    public getRecommended(): Observable<any> {
        return this.http.get('/Movies/Recommended');
    } 
    
    public getFavorites(skip:number, count:number): Observable<any> {
        return this.http.get(`/Movies/Favorites?skip=${skip}&count=${count}`);
    }

    public getMovieByImdbId(imdbId: string): Observable<any> {
        return this.http.get(`/Movies/ImdbId/${imdbId}`);
    }

    public createMovieFromImdbId(imdbIdDto: ImdbIdDto): Observable<any> {
        return this.http.post('/Movies', imdbIdDto);
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