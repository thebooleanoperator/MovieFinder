import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MoviesService } from '../Services/movies.service';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ResolvedFavoriteMovies } from 'src/app/Data/ResolvedData/resolved-favorite-movies';
import { UserService } from '../Services/user.service';

@Injectable()
export class FavoriteMoviesResolver implements Resolve<ResolvedFavoriteMovies> {
    constructor(private _moviesService: MoviesService, private _userService: UserService) {}

    resolve(): Observable<ResolvedFavoriteMovies> {
        if(this._userService.isGuest()) {
           return of(new ResolvedFavoriteMovies(null, null)); 
        }
        else {
            return this._moviesService.getFavorites(0, 30)
            .pipe(
                map((favoriteMovies: MovieDto[]) => new ResolvedFavoriteMovies(favoriteMovies)),
                catchError((error: any) => of(new ResolvedFavoriteMovies(null, error)))
            );
        }
    }
}
