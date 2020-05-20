import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { ToolBarService } from '../Services/tool-bar.service';
import { FavoritesService } from '../Services/favorites.service';
import { FavortiesDto } from 'src/app/Data/favorites.dto';

@Injectable()
export class FavoritesResolver implements Resolve<FavortiesDto[]> {
  constructor(private _favoritesService: FavoritesService, private _toolBarService: ToolBarService) {}

  resolve(): Promise<any> {
    this._toolBarService.isLoading = true;
    return this._favoritesService.getFavoriteMovies().toPromise()
        .then((favoriteDtos) => {
            return favoriteDtos;
        })
        .catch((error) => {
            if (error.status != 401) {
                alert("Could not load favorites list.");
            }
        })
        .finally(() => this._toolBarService.isLoading = false);
  }
}