import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { ToolBarService } from '../Services/tool-bar.service';
import { FavoritesService } from '../Services/favorites.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { Observable, of, throwError } from 'rxjs';
import { map, finalize, catchError } from 'rxjs/operators';
import { ResolvedFavorites } from 'src/app/Data/ResolvedData/resolved-favorites';

@Injectable()
export class FavoritesResolver implements Resolve<ResolvedFavorites> {
    constructor(
        private _favoritesService: FavoritesService, 
        private _toolBarService: ToolBarService) {}

    resolve(): Observable<ResolvedFavorites> {
    this._toolBarService.isLoading = true;
    return this._favoritesService.getFavoriteMovies()
        .pipe (
            map((favorites: FavortiesDto[]) => new ResolvedFavorites(favorites)),
            catchError((error: any) => of(new ResolvedFavorites(null, error))),
            finalize(() => this._toolBarService.isLoading = false)
        )
    }
}