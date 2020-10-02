import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';

@Injectable({providedIn: 'root'})

export class FavoritesService {
    
    constructor(private http: HttpClient){}

    private _favoritesAdded = new Subject();
    favoritesAdded$ = this._favoritesAdded.asObservable();

    private _favoritesRemoved = new Subject();
    favoritesRemoved$ = this._favoritesAdded.asObservable();
    // Event Emitters
    favoriteAdded(favorite: FavortiesDto) {
        this._favoritesAdded.next(favorite);
    }

    favoriteRemoved(favorite: FavortiesDto) {
        this._favoritesRemoved.next(favorite);
    }

    // Http Calls
    saveFavorite(favoritesDto: FavortiesDto): Observable<any> {
        return this.http.post(`/LikedMovies`, favoritesDto);
    }

    getByMovieId(movieId: number): Observable<any> {
        return this.http.get(`/LikedMovies/${movieId}`);
    }

    getAll(skip: number, count: number): Observable<any> {
        return this.http.get(`/LikedMovies?skip=${skip}&count=${count}`);
    }

    deleteFavorite(favoriteId: number): Observable<any> {
        return this.http.delete(`/LikedMovies/${favoriteId}`);
    }
}