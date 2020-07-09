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
export class SearchHistoryComponent implements OnInit, OnChanges {
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
            this.onWideScreen = false;
            this.displayedMovies = this.createDisplayedSearchHistory(this.searchIndex, this.searchedMovies, this.onWideScreen);
        }
        else if (event.target.innerWidth > 1000 && !this.onWideScreen) {
            this.onWideScreen = true;
            this.displayedMovies = this.createDisplayedSearchHistory(this.searchIndex, this.searchedMovies, this.onWideScreen);
        }
    }

    displayedMovies: MovieDto[]; 
    searchIndex: number = 0;
    onWideScreen: boolean = window.innerWidth > 1000; 


    ngOnInit() {
        if (this.searchedMovies.length > 0) {
            this.createDisplayedSearchHistory(this.searchIndex, this.searchedMovies, this.onWideScreen); 
        }
    }

    ngOnChanges() {
        if (this.searchedMovies.length > 0) {
            this.searchIndex = 0; 
            this.displayedMovies = this.createDisplayedSearchHistory(this.searchIndex, this.searchedMovies, this.onWideScreen);
        }   
    }

    moveSearchIndex(increment: number, searchedMovies: MovieDto[], onWideScreen: boolean) {
        this.searchIndex = this.setSearchIndex(increment, searchedMovies);
        this.displayedMovies = this.createDisplayedSearchHistory(this.searchIndex, searchedMovies, onWideScreen); 
    }

    setSearchIndex(increment: number, searchedMovies) {
        var idx = this.searchIndex += increment;
        if (idx < 0) {
            return searchedMovies.length - 1;
        }
        if (idx >= searchedMovies.length) {
            return 0;
        }
        return idx;
    }

    createDisplayedSearchHistory(searchIdx: number, searchedMovies: MovieDto[], onWideScreen: boolean) : MovieDto[] {
        if (!onWideScreen) {
            return [searchedMovies[searchIdx]];
        }
        var idx1 = searchIdx + 1 < searchedMovies.length ? searchIdx + 1 : 0;
        var idx2 = idx1 + 1 < searchedMovies.length ? idx1 + 1 : 0; 

        return [searchedMovies[searchIdx], searchedMovies[idx1], searchedMovies[idx2]];
    }

    disableMoveSearchIndex(searchedMovies: MovieDto[]) {
        return searchedMovies.length <= 1;
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

    useDefaultPoster(event) {
        event.srcElement.src = "/assets/images/default-poster.png";
    }
}