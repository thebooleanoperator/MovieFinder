import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { MovieDto } from 'src/app/Data/movie.dto';
import { MoviesService } from '../Services/movies.service';
import { ToolBarService } from '../Services/tool-bar.service';

@Injectable()
export class MoviesResolver implements Resolve<MovieDto[]> {
  constructor(private _moviesService: MoviesService, private _toolBarService: ToolBarService) {}

  resolve(): Promise<any> {
    this._toolBarService.isLoading = true;
    return this._moviesService.getRecommended().toPromise()
        .then((movieDtos) => {
            return movieDtos;
        })
        .catch((error) => {
            if (error.status != 401) {
                alert("Could not load recommended movies.");
            }
        })
        .finally(() => this._toolBarService.isLoading = false);
  }
}
