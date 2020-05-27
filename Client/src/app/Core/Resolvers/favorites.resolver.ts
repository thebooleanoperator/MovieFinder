import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { FavoritesService } from '../Services/favorites.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ResolvedFavorites } from 'src/app/Data/ResolvedData/resolved-favorites';

@Injectable()
export class FavoritesResolver implements Resolve<ResolvedFavorites> {
    constructor(private _favoritesService: FavoritesService) {}

    resolve(): Observable<ResolvedFavorites> {
    return this._favoritesService.getFavoriteMovies()
        .pipe (
            map((favorites: FavortiesDto[]) => new ResolvedFavorites(favorites)),
            catchError((error: any) => of(new ResolvedFavorites(null, error)))
        )
    }
}