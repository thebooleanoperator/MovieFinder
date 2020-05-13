import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { MovieDto } from 'src/app/Data/movie.dto';
import { MoviesService } from '../Services/movies.service';
import { ToolBarService } from '../Services/tool-bar.service';

@Injectable()
export class FavoriteMoviesResolver implements Resolve<MovieDto[]> {
  constructor(private _moviesService: MoviesService, private _toolBarService: ToolBarService) {}

  resolve(): Promise<any> {
    this._toolBarService.isLoading = true;
    return this._moviesService.getFavorites(0, 20).toPromise()
        .then((favoriteMovieDtos) => {
            return favoriteMovieDtos;
        })
        .catch(() => alert("Could not load favorite movies."))
        .finally(() => this._toolBarService.isLoading = false);
  }
}
