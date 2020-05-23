import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { ToolBarService } from '../Services/tool-bar.service';
import { FavoritesService } from '../Services/favorites.service';
import { FavortiesDto } from 'src/app/Data/favorites.dto';
import { Observable } from 'rxjs';
import { map, finalize } from 'rxjs/operators';

@Injectable()
export class FavoritesResolver implements Resolve<FavortiesDto[]> {
    constructor(
        private _favoritesService: FavoritesService, 
        private _toolBarService: ToolBarService) {}

    resolve(): Observable<FavortiesDto[]> {
    this._toolBarService.isLoading = true;
    return this._favoritesService.getFavoriteMovies()
        .pipe (
            map((favorites: FavortiesDto[]) => favorites),
            finalize(() => this._toolBarService.isLoading = false)
        )
    }
}