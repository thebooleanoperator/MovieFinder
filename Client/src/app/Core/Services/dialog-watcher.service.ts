import { Subject } from 'rxjs';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { Injectable } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { UserService } from './user.service';

@Injectable({providedIn: 'root'})
/**
 * Service that creates subjects for events from DialogWatcherService.
 */
export class DialogWatcherService {
    constructor(private _userService: UserService){};

    closeEventFavorites$ = new Subject<FavortiesDto[]>();
    closeEventMovie$ = new Subject<MovieDto>(); 

    /**
     * Sends observable stream to subscribers from favorites$ and movie$ subjects
     * @param favroites 
     * @param selectedMovie 
     * @param updateSearchHistory 
     */
    closedByClickOutside(favroites: FavortiesDto[], selectedMovie: MovieDto, updateSearchHistory: boolean) {
        if (!this._userService.isGuest()) {
            this.closeEventFavorites$.next(favroites);
            if (updateSearchHistory) {
                this.closeEventMovie$.next(selectedMovie); 
            }
        }
    }
}