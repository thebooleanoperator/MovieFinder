import { Subject } from 'rxjs';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { Injectable } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';

@Injectable({providedIn: 'root'})
/**
 * Service that creates an observable stream for a dialog closing. 
 * This is used so data can be sent to parent componenet when user clicks outside to close modal.
 */
export class DialogWatcherService {
    closeEventFavorites$ = new Subject<FavortiesDto[]>();
    closeEventMovie$ = new Subject<MovieDto>(); 

    closedByClickOutside(favroites: FavortiesDto[], selectedMovie: MovieDto) {
        this.closeEventFavorites$.next(favroites);
        this.closeEventMovie$.next(selectedMovie); 
    }
}