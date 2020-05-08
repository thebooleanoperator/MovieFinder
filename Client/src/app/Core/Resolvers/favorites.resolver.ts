import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { MovieDto } from 'src/app/Data/movie.dto';
import { ToolBarService } from '../Services/tool-bar.service';
import { FavoritesService } from '../Services/favorites.service';

@Injectable()
export class FavoritesResolver implements Resolve<MovieDto[]> {
  constructor(private _favoritesService: FavoritesService, private _toolBarService: ToolBarService) {}

  resolve(): Promise<any> {
    this._toolBarService.isLoading = true;
    return this._favoritesService.getFavoriteMovies().toPromise()
        .then((favoriteDtos) => {
            return favoriteDtos;
        })
        .catch(() => alert("Could not load recommended movies."))
        .finally(() => this._toolBarService.isLoading = false);
  }
}