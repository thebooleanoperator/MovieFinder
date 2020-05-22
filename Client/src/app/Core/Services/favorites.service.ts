import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { FavortiesDto } from 'src/app/Data/favorites.dto';

@Injectable({providedIn: 'root'})

export class FavoritesService {
    
    constructor(private http: HttpClient){}

    saveFavorite(favoritesDto: FavortiesDto): Observable<any> {
        return this.http.post(`http://localhost:5001/LikedMovies`, favoritesDto);
    }

    getFavoriteMovies(): Observable<any> {
        return this.http.get(`http://localhost:5001/LikedMovies`);
    }

    deleteFavorite(favoriteId: number): Observable<any> {
        return this.http.delete(`http://localhost:5001/LikedMovies/${favoriteId}`);
    }
}