import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { MovieDto } from 'src/app/Data/movie.dto';
import { MoviesService } from '../Services/movies.service';
import { ToolBarService } from '../Services/tool-bar.service';
import { Observable } from 'rxjs';
import { map, finalize } from 'rxjs/operators';

@Injectable()
export class FavoriteMoviesResolver implements Resolve<MovieDto[]> {
    constructor(
        private _moviesService: MoviesService, 
        private _toolBarService: ToolBarService) {}

    resolve(): Observable<MovieDto[]> {
        this._toolBarService.isLoading = true;
        return this._moviesService.getFavorites(0, 30)
            .pipe(
                map((favoriteMovies: MovieDto[]) => favoriteMovies),
                finalize(() => this._toolBarService.isLoading = false)
            );
    }
}
