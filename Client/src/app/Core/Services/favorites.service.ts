import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';

@Injectable({providedIn: 'root'})

export class FavoritesService {
    
    constructor(private http: HttpClient){}

    saveFavorite(favoritesDto: FavortiesDto): Observable<any> {
        return this.http.post(`/LikedMovies`, favoritesDto);
    }

    getFavoriteMovies(): Observable<any> {
        return this.http.get(`/LikedMovies`);
    }

    deleteFavorite(favoriteId: number): Observable<any> {
        return this.http.delete(`/LikedMovies/${favoriteId}`);
    }
}