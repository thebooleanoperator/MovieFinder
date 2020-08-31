import { Component, Input, OnChanges, HostListener, OnInit } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MatDialog } from '@angular/material/dialog';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { SearchHistoryDto } from 'src/app/Data/Interfaces/search-history.dto';
import { MoviesService } from 'src/app/Core/Services/movies.service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';

@Component({
    selector: 'search-history',
    templateUrl: './search-history.component.html',
    styleUrls: ['./search-history.component.scss']
})
export class SearchHistoryComponent implements OnInit, OnChanges {
    constructor(
        private _dialog: MatDialog,
        private _dialogWatcher: DialogWatcherService,
        private _moviesService: MoviesService,
        private _toolBarService: ToolBarService
    ){}

    @Input() isGuest: boolean;
    @Input() favorites: FavortiesDto[];
    @Input() searchedMovies: SearchHistoryDto[];
    @Input() searchTableDisplayed: boolean;

    @HostListener('window:resize', ['$event'])
    onResize(event) {
        if (!this.isGuest) {
            if (event.target.innerWidth <= 1000 && this.onWideScreen) {
                this.onWideScreen = false;
                this.displayedMovies = this.createDisplayedSearchHistory(this.searchIndex, this.searchedMovies, this.onWideScreen);
            }
            else if (event.target.innerWidth > 1000 && !this.onWideScreen) {
                this.onWideScreen = true;
                this.displayedMovies = this.createDisplayedSearchHistory(this.searchIndex, this.searchedMovies, this.onWideScreen);
            }
        }
    }

    displayedMovies: SearchHistoryDto[]; 
    searchIndex: number = 0;
    onWideScreen: boolean = window.innerWidth > 1000; 

    ngOnInit() {
        if (!this.isGuest && this.searchedMovies.length > 0) {
            this.createDisplayedSearchHistory(this.searchIndex, this.searchedMovies, this.onWideScreen); 
        }
    }

    ngOnChanges() {
        if (!this.isGuest) {
            this.setDisplayedMovies(this.searchedMovies, this.onWideScreen, this.searchIndex);
            // We need to set the search index back to 0 if there is a search history.
            if (this.searchedMovies.length > 0) {
                this.searchIndex = 0;
            }
        }
    }

    moveSearchIndex(increment: number, searchedMovies: SearchHistoryDto[], onWideScreen: boolean) {
        this.searchIndex = this.setSearchIndex(increment, searchedMovies);
        this.setDisplayedMovies(searchedMovies, onWideScreen, this.searchIndex);
    }

    setDisplayedMovies(searchHistory: SearchHistoryDto[], onWideScreen: boolean, searchIndex: number) {
        switch (searchHistory.length) {
            case 0:
                break;
            case 1: 
                this.displayedMovies = searchHistory;
                break;
            case 2:
                this.displayedMovies = searchHistory;
                break;
            default:
                this.displayedMovies = this.createDisplayedSearchHistory(searchIndex, searchHistory, onWideScreen);
        } 
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

    createDisplayedSearchHistory(searchIdx: number, searchedMovies: SearchHistoryDto[], onWideScreen: boolean) : SearchHistoryDto[] {
        if (!onWideScreen) {
            return [searchedMovies[searchIdx]];
        }
        var idx1 = searchIdx + 1 < searchedMovies.length ? searchIdx + 1 : 0;
        var idx2 = idx1 + 1 < searchedMovies.length ? idx1 + 1 : 0; 

        return [searchedMovies[searchIdx], searchedMovies[idx1], searchedMovies[idx2]];
    }

    disableMoveSearchIndex(searchedMovies: SearchHistoryDto[]) {
        return searchedMovies.length <= 1;
    }

    getMovieAndOpenDialog(moveId: number, favorites: FavortiesDto[]): void {
        this._toolBarService.isLoading = true
        this._moviesService.get(moveId)
            .subscribe(
                ((movieDto: MovieDto) =>{
                    if (movieDto) {
                        this.openMovieDialog(movieDto, favorites);
                    }
                    else {
                        alert("Could not find movie.");
                    }
                }),
                ((error) => {
                    if (error.status !== 401) {
                        alert("Failed to get movie")
                    }
                }),
                (() => this._toolBarService.isLoading = false)
            )
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