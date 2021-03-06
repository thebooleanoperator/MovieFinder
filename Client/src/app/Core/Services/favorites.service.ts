import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';

@Injectable({providedIn: 'root'})

export class FavoritesService {
    
    constructor(private http: HttpClient){}

    private _favoritesUpdated = new Subject();
    favoritesUpdated$ = this._favoritesUpdated.asObservable();

    // Event Emitters
    favoritesUpdated(favoriteDto: FavortiesDto) {
        this._favoritesUpdated.next(favoriteDto);
    }

    // Http Methods
    saveFavorite(favoritesDto: FavortiesDto): Observable<any> {
        return this.http.post(`/LikedMovies`, favoritesDto);
    }

    getByMovieId(movieId: number): Observable<any> {
        return this.http.get(`/LikedMovies/${movieId}`);
    }

    getAll(skip: number, count: number): Observable<any> {
        return this.http.get(`/LikedMovies?skip=${skip}&count=${count}`);
    }

    deleteFavorite(movieId: number): Observable<any> {
        return this.http.delete(`/LikedMovies/${movieId}`);
    }
}
