import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MoviesService } from '../Services/movies.service';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ResolvedFavoriteMovies } from 'src/app/Data/ResolvedData/resolved-favorite-movies';

@Injectable()
export class FavoriteMoviesResolver implements Resolve<ResolvedFavoriteMovies> {
    constructor(private _moviesService: MoviesService) {}

    resolve(): Observable<ResolvedFavoriteMovies> {
        return this._moviesService.getFavorites(0, 30)
            .pipe(
                map((favoriteMovies: MovieDto[]) => new ResolvedFavoriteMovies(favoriteMovies)),
                catchError((error: any) => of(new ResolvedFavoriteMovies(null, error)))
            );
    }
}
