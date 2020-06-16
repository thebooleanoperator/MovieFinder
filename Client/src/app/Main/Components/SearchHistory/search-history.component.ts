import { Component, Input, OnChanges } from '@angular/core';
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
export class SearchHistoryComponent implements OnChanges {
    constructor(
        private _dialog: MatDialog,
        private _dialogWatcher: DialogWatcherService
    ){}

    @Input() favorites: FavortiesDto[];
    @Input() searchedMovies: MovieDto[];
    @Input() searchTableDisplayed: boolean;

    displayedMovies: MovieDto[]; 
    startingIndex: number = 0;
    endingIndex: number = 2;

    /**
     * 
     */
    ngOnChanges() {
        if (this.searchedMovies) {
            this.displayedMovies = this.searchedMovies.slice(this.startingIndex, this.endingIndex+1);
            // Reassign ending index in case there are not three searched movies.
            this.endingIndex = this.displayedMovies.length - 1; 
        }   
    }

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    openMovieDialog(movie, favorites) {
        var isFavorite = this.getIsFavorite(movie, favorites);
        this._dialog.open(SelectedMovieDialog, {
            data: {movie: movie, favoriteMovies: favorites, isFavorite: isFavorite, updateSearchHistory: false}
        });

        this._dialogWatcher.closeEventFavorites$.subscribe(
            (favorites) => this.favorites = favorites
        );
    }

    navigateSearchedMovies(increment: number): void {
        this.adjustIndexes(increment);

        this.displayedMovies = this.searchedMovies.slice(this.startingIndex, this.endingIndex+1);
    }

    adjustIndexes(increment: number): void {
        this.startingIndex += increment;
        this.endingIndex += increment;
    }

    nextExists(): boolean {
        return this.endingIndex + 1 < this.searchedMovies.length;
    }

    previousExists(): boolean {
        return this.endingIndex > 2; 
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