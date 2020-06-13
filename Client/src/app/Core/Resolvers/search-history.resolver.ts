import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MoviesService } from '../Services/movies.service';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ResolvedSearchHistory } from 'src/app/Data/ResolvedData/resolved-search-history';

@Injectable()
export class SearchHistoryResolver implements Resolve<ResolvedSearchHistory> {
    constructor(private _moviesService: MoviesService) {}

    resolve(): Observable<ResolvedSearchHistory> {
        return this._moviesService.getMovieSearchHistory(10)
            .pipe(
                map((movieSearchHistory: MovieDto[]) => new ResolvedSearchHistory(movieSearchHistory)),
                catchError((error: any) => of(new ResolvedSearchHistory(null, error)))
            );
    }
}