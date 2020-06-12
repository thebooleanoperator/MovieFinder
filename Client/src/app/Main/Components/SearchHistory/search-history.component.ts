import { Component, Input } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MatDialog } from '@angular/material/dialog';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';

@Component({
    selector: 'search-history',
    templateUrl: './search-history.component.html',
    styleUrls: ['./search-history.component.scss']
})
export class SearchHistoryComponent {
    constructor(
        private _dialog: MatDialog,
        private _dialogWatcher: DialogWatcherService
    ){}

    @Input() favorites: FavortiesDto[];
    @Input() searchedMovies: MovieDto[];
    @Input() searchTableDisplayed: boolean;

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    openMovieDialog(movie, favorites) {
        var isFavorite = this.getIsFavorite(movie, favorites);
        this._dialog.open(SelectedMovieDialog, {
            data: {movie: movie, favoriteMovies: favorites, isFavorite: isFavorite}
        });

        this._dialogWatcher.$closeEvent.subscribe((favorites) => {
            this.favorites = favorites;
        })
    }

    /**
     * 
     * @param movie 
     * @param favorites 
     */
    getIsFavorite(movie: MovieDto, favorites: FavortiesDto[]): boolean {
        if (!favorites) {
            return false;
        }

        return favorites.some((favorite) => {
            return favorite.movieId == movie.movieId;
        })
    }

    historyExists(searchedMovies) {
        if (!searchedMovies) {
            return false;
        }

        return searchedMovies.length > 0; 
    }
}