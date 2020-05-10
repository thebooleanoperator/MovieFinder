import { Subject } from 'rxjs';
import { FavortiesDto } from 'src/app/Data/favorites.dto';
import { Injectable } from '@angular/core';

@Injectable({providedIn: 'root'})
/**
 * Service that creates an observable stream for a dialog closing. 
 * This is used so data can be sent to parent componenet when user clicks outside to close modal.
 */
export class DialogWatcherService {
    closeEvent$ = new Subject<FavortiesDto[]>();
    closedByClickOutside(favroites: FavortiesDto[]) {
        this.closeEvent$.next(favroites);
    }
}