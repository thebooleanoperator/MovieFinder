import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MoviesService } from '../Services/movies.service';
import { map, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { ResolvedMovies } from 'src/app/Data/ResolvedData/resolved-movies';

@Injectable()
export class MoviesResolver implements Resolve<ResolvedMovies> {
  constructor(private _moviesService: MoviesService) {}

  resolve(): Observable<ResolvedMovies> {
    return this._moviesService.getRecommended()
        .pipe(
            map((recommendedMovies: MovieDto[]) => new ResolvedMovies(recommendedMovies)),
            catchError((error) => of(new ResolvedMovies(null, error)))
        )
  }
}
