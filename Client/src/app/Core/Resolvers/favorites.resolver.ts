import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { FavoritesService } from '../Services/favorites.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ResolvedFavorites } from 'src/app/Data/ResolvedData/resolved-favorites';
import { UserService } from '../Services/user.service';

@Injectable()
export class FavoritesResolver implements Resolve<ResolvedFavorites> {
    constructor(private _favoritesService: FavoritesService, private _userService: UserService) {}
    
    resolve(): Observable<ResolvedFavorites> {
        if (this._userService.isGuest()) {
            return of(new ResolvedFavorites(null, null));
        }
        else {
            // Always resolve the first 10 favorites.
            return this._favoritesService.getAll(0, 10)
                .pipe (
                    map((favorites: FavortiesDto[]) => new ResolvedFavorites(favorites)),
                    catchError((error: any) => of(new ResolvedFavorites(null, error)))
                )
        }
    }
}