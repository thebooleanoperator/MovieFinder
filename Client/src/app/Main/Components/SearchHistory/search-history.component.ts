import { Component, Input, OnChanges, HostListener, OnInit } from '@angular/core';
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

    @HostListener('window:resize', ['$event'])
    onResize(event) {
        if (event.target.innerWidth <= 1000 && this.onWideScreen) {
            this.displayedMovies = [this.searchedMovies[this.startingIndex]];
            this.onWideScreen = false;
        }
        else if (event.target.innerWidth > 1000 && !this.onWideScreen) {
            this.adjustIndexes(this.searchedMovies);
            this.displayedMovies = this.searchedMovies.slice(this.startingIndex, this.endingIndex+1);
            this.onWideScreen = true;
        }
    }

    displayedMovies: MovieDto[]; 
    startingIndex: number = 0;
    endingIndex: number = 2;
    onWideScreen: boolean = window.innerWidth > 1000; 
    
    /**
     * 
     */
    ngOnChanges() {
        if (this.searchedMovies) {
            this.onWideScreen ? this.adjustIndexes(this.searchedMovies) : null;
            this.displayedMovies = this.onWideScreen
                ? this.searchedMovies.slice(this.startingIndex, this.endingIndex+1)
                : [this.searchedMovies[0]];
            // Reassign ending index in case there are not three searched movies.
            this.endingIndex = this.displayedMovies.length - 1; 
        }   
    }

    adjustIndexes(searchedMovies: MovieDto[]) {
        var totalHistory = searchedMovies.length;
        if (this.startingIndex + 2 < totalHistory) {
            this.endingIndex = this.startingIndex + 2; 
        }
        else {
            var calibratingIndexes = true; 
            while (calibratingIndexes) {
                if (this.startingIndex - 1 >= 0) {
                    this.startingIndex -= 1; 
                    if (this.startingIndex + 2 < totalHistory) {
                        this.endingIndex = this.startingIndex + 2;
                        calibratingIndexes = false;
                    }
                }
                else {
                    this.startingIndex = 0;
                    this.endingIndex = totalHistory - 1;
                    calibratingIndexes = false;
                }
            }
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

    navigateSearchedMovies(increment: number, onWideScreen: boolean): void {
        onWideScreen ? this.incrementIndexes(increment) : this.startingIndex += increment;
        
        this.displayedMovies = onWideScreen 
            ? this.searchedMovies.slice(this.startingIndex, this.endingIndex+1)
            : [this.searchedMovies[this.startingIndex]];
    }

    incrementIndexes(increment: number): void {
        this.startingIndex += increment;
        this.endingIndex += increment;
    }

    nextExists(searchedMovies: MovieDto[], startingIndex: number, endingIndex: number, onWideScreen: boolean): boolean {
        return onWideScreen ? endingIndex + 1 < searchedMovies.length : startingIndex + 1 < searchedMovies.length;
    }

    previousExists(startingIndex: number, endingIndex: number, onWideScreen: boolean): boolean {
        return onWideScreen ? endingIndex > 2 : startingIndex > 0; 
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