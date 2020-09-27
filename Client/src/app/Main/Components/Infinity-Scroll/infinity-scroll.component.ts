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
    selector: 'infinity-scroll',
    templateUrl: './infinity-scroll.component.html',
    styleUrls: ['./infinity-scroll.component.scss']
})
export class InfinityScrollComponent implements OnInit, OnChanges {
    constructor(
        private _dialog: MatDialog,
        private _dialogWatcher: DialogWatcherService,
        private _moviesService: MoviesService,
        private _toolBarService: ToolBarService)
    {

    }

    @Input() isGuest: boolean;
    @Input() favorites: FavortiesDto[];
    @Input() movies: SearchHistoryDto[];

    @HostListener('window:resize', ['$event'])
    onResize(event) {
        if (!this.isGuest) {
            if (event.target.innerWidth <= 1000 && this.onWideScreen) {
                this.onWideScreen = false;
                this.displayedMovies = this.createDisplayedSearchHistory(this.searchIndex, this.movies, this.onWideScreen);
            }
            else if (event.target.innerWidth > 1000 && !this.onWideScreen) {
                this.onWideScreen = true;
                this.displayedMovies = this.createDisplayedSearchHistory(this.searchIndex, this.movies, this.onWideScreen);
            }
        }
    }

    displayedMovies: SearchHistoryDto[]; 
    searchIndex: number = 0;
    onWideScreen: boolean = window.innerWidth > 1000; 

    ngOnInit() {
        if (!this.isGuest && this.movies.length > 0) {
            this.createDisplayedSearchHistory(this.searchIndex, this.movies, this.onWideScreen); 
        }
    }

    ngOnChanges() {
        if (!this.isGuest) {
            this.setDisplayedMovies(this.movies, this.onWideScreen, this.searchIndex);
            // We need to set the search index back to 0 if there is a search history.
            if (this.movies.length > 0) {
                this.searchIndex = 0;
            }
        }
    }

    moveSearchIndex(increment: number, movies: SearchHistoryDto[], onWideScreen: boolean) {
        this.searchIndex = this.setSearchIndex(increment, movies);
        this.setDisplayedMovies(movies, onWideScreen, this.searchIndex);
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

    setSearchIndex(increment: number, movies) {
        var idx = this.searchIndex += increment;
        if (idx < 0) {
            return movies.length - 1;
        }
        if (idx >= movies.length) {
            return 0;
        }
        return idx;
    }

    createDisplayedSearchHistory(searchIdx: number, movies: SearchHistoryDto[], onWideScreen: boolean) : SearchHistoryDto[] {
        if (!onWideScreen) {
            return [movies[searchIdx]];
        }
        var idx1 = searchIdx + 1 < movies.length ? searchIdx + 1 : 0;
        var idx2 = idx1 + 1 < movies.length ? idx1 + 1 : 0; 

        return [movies[searchIdx], movies[idx1], movies[idx2]];
    }

    disableMoveSearchIndex(movies: SearchHistoryDto[]) {
        return movies.length <= 1;
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

    historyExists(movies) {
        if (!movies) {
            return false;
        }

        return movies.length > 0; 
    }

    useDefaultPoster(event) {
        event.srcElement.src = "/assets/images/default-poster.png";
    }    
}