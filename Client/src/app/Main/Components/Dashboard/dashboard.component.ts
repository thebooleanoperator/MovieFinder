import { Component, OnInit, ElementRef, ViewChild, AfterViewInit, OnDestroy, Output } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MoviesService } from 'src/app/Core/Services/movies.service';
import { ImdbIdDto } from 'src/app/Data/Interfaces/imdbId.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { MatDialog } from '@angular/material/dialog';
import { ImdbIdsService } from 'src/app/Core/Services/imdbIds.service';
import { ActivatedRoute } from '@angular/router';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';
import { fromEvent, Subscription } from 'rxjs';
import { map, debounceTime, distinctUntilChanged, switchMap, concatMap } from 'rxjs/operators';
import { SearchHistoryService } from 'src/app/Core/Services/search-history.service';
import { SearchHistoryDto } from 'src/app/Data/Interfaces/search-history.dto';
import { UserService } from 'src/app/Core/Services/user.service';
import { InfitiyScrollDto } from 'src/app/Data/Interfaces/infinity-scroll.dto';
import { TooltipComponent } from '@angular/material/tooltip';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {
    constructor(
        private _route: ActivatedRoute,
        private _dialogWatcher: DialogWatcherService,
        private _moviesService: MoviesService, 
        private imdbIdsService: ImdbIdsService, 
        private _searchHistoryService: SearchHistoryService,
        private _toolBarService: ToolBarService,
        private _userService: UserService,
        private dialog: MatDialog){}

    /**
     * Holds an array of all movies returned from search results.
     */
    movies: MovieDto[];
    /**
     * 
     */
    recommendedMovies: MovieDto[];
    /**
     * 
     */
    favoriteMovies: MovieDto[];
    /**
     * The string being entered into the search bar. Gets sent to server to find movie.
     */
    search: string;
    /**
     * Used to filter search by year.
     */
    year: number;
    /**
     * Holds all of a users favorited movies.
     */
    favorites: FavortiesDto[];
    /**
     * Holds the array of the movies being shown on current page. Used for client side paging.
     */
    displayedMovies: MovieDto[];
    /**
     * 
     */
    searchedMovies: SearchHistoryDto[];
    /**
     * The movie a user has selected from the search results. 
     */
    selectedMovie: MovieDto;
    /**
     * Toggles the No Search Results found message in view.
     */
    noSearchResults: boolean = false;
    /**
     * Holds the timeout search function.
     */
    timeout: NodeJS.Timer;
    /**
     * Column titles for search results table.
     */
    displayedColumns : string[] = ['Title', 'Year'];
    /**
     * Used to disable input search when a user is selecting a movie.
     */
    gettingMovie: boolean = false;
    /**
     * Used to set the number of movies displayed from search results.
     */
    moviesPerPage: number = 8;
    /**
     * Holds the total number of pages of search results returned by the server.
     */
    totalPages: number;
    /**
     * 
     */
    searchTableDisplayed: boolean = false;
    /**
     * 
     */
    routerSubscription: Subscription;
    /**
     *
     */
    dialogFavoritesSubscription: Subscription; 
    /**
     * 
     */
    dialogMovieSubscription: Subscription;
    /**
     * Holds the callback to correctly page the movie results.
     */
    pageEvent: any = 1; 
    /**
     * True if a catch block in a promise has been entered. 
     * Don't want to show multiple alerts to user for 1 request.
     */
    error: string[] = [];
    /**
     * 
     */
    isGuest: boolean = this._userService.isGuest();

    @ViewChild('imdbIdSearch', null) imdbIdSearch: ElementRef;

    /**
     * Subscribe to resolver and handle error if one occurs.
     */
    ngOnInit() {
        // Router subscription
        this.routerSubscription = this._route.data.subscribe(
            (data) => {
                var favoritesResolverError = data.resolvedFavorites.error;
                var searchHistorResolverError = data.resolvedSearchHistory.error; 
                var recommendedMoviesResolverError = data.resolvedMovies.error;
                var favoriteMoviesResolverError = data.resolvedFavoriteMovies.error;
                if (!favoritesResolverError) {
                    this.favorites = data.resolvedFavorites.favorites;
                }
                if (!searchHistorResolverError) {
                    this.searchedMovies = data.resolvedSearchHistory.searchHistory
                        ? data.resolvedSearchHistory.searchHistory 
                        : [];
                }
                if (!recommendedMoviesResolverError) {
                    this.recommendedMovies = data.resolvedMovies.movies;
                }
                if (!favoriteMoviesResolverError) {
                    this.favoriteMovies = data.resolvedFavoriteMovies.favoriteMovies;
                }
                else {
                    if (favoritesResolverError.status != 401) {
                        this.error.push(favoritesResolverError);
                        this.error.push(searchHistorResolverError);
                        this.error.push(recommendedMoviesResolverError);
                        this.error.push(favoriteMoviesResolverError);
                    }
                }
            }
        )

        // Favoties Subscription
        this.dialogFavoritesSubscription = this._dialogWatcher.closeEventFavorites$.subscribe(
            (favorites) => this.favorites = favorites
        );

        // SearchHistory subscription
        this.dialogMovieSubscription = this._dialogWatcher.closeEventMovie$.subscribe( 
            (movie) => {
                var searchHistory = new SearchHistoryDto(movie, this._userService.getUser());
                this._searchHistoryService.create(searchHistory)
                    .pipe(
                        concatMap(() => this._searchHistoryService.getAll(20))
                    )
                    .subscribe(
                        (searchedMovies: SearchHistoryDto[]) => this.searchedMovies = searchedMovies,
                        ((error) => {
                            if (error.status !== 401) {
                                alert("Search history failed to update"); 
                            }
                        })
                    )
            }
        )
    }

    /**
     * All subject subscriptions need to be unsubscribed from. 
     */
    ngOnDestroy() {
        try {
            this.routerSubscription.unsubscribe();
            this.dialogFavoritesSubscription.unsubscribe();
            this.dialogMovieSubscription.unsubscribe();
        }
        catch(error) {
            console.log('Error: ' + error);
        } 
    }

    /**
     * Listens for output from InfinityScrollComponent to page history or favorites.
     * @param infinityScrollDto 
     */
    onGetNextMovies(infinityScrollDto: InfitiyScrollDto): Subscription {
        switch(infinityScrollDto.typeScrolled) {
            case "favorites":
                return this.getNextFavorites(infinityScrollDto.skip, infinityScrollDto.count);
        }
    }   

    /**
     * 
     * @param skip 
     * @param count 
     */
    getNextFavorites(skip: number, count: number): Subscription {
        this._toolBarService.isLoading = true;
            return this._moviesService.getFavorites(skip, count)
                .subscribe (
                    (favoriteMoviesDtos) => {
                        if (favoriteMoviesDtos) {
                            this.favoriteMovies = this.favoriteMovies.concat(favoriteMoviesDtos);
                        }
                    },
                    (error) => {
                        alert("Unable to load favorites.");
                        console.log(error);
                    },
                    () => this._toolBarService.isLoading = false
                );
    }

    onAddFavorites() {
        
    }

    addToFavorites(movie: MovieDto): void {
        var favorite: FavortiesDto = new FavortiesDto(movie, this._authService.user);
        this._toolBarService.isLoading = true;
        this._favoritesService.saveFavorite(favorite)
            .subscribe(
                (data) => {
                    this.favorites ? this.favorites.push(data) : this.favorites = [data];
                    // Emit to parent that favoriteMovies has been changed.
                    this.favoriteAdded.emit(this.favorites); 
                },
                (error) => {
                    if (error.status != 401) {
                        alert("Failed to add movie to favorites.");
                    }
                    this._toolBarService.isLoading = false;
                },
                () => this._toolBarService.isLoading = false
            )
    }

    /**
     * Sets the total pages variable. Used for client side paging.
     * @param totalMovies 
     * @param moviesPerPage 
     */
    setTotalPages(totalMovies, moviesPerPage): void {
        if (totalMovies > 0) {
            this.totalPages = Math.ceil(totalMovies / moviesPerPage);
        }
    }

    /**
     * Used to get isFavorite value to send to dialog.
     * @param movie 
     * @param favoriteMovies 
     */
    getIsFavorite(movie: MovieDto, favoriteMovies: FavortiesDto[]): boolean {
        if (!favoriteMovies) {
            return false;
        }

        return favoriteMovies.some((favorite) => {
            return favorite.movieId == movie.movieId;
        })
    }

    /**
     * Toggles error message in template if errors took place while resolving data.
     * @param error 
     */
    isError(error: any[]) {
        if (!error) {
            return false;
        }
        return error.length > 0 ? true : false;
    }

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    private openDialog(movie, favoriteMovies, isGuest) {
        var isFavorite = this.getIsFavorite(movie, favoriteMovies);
        var existsInHistory = this.searchedMovies.some((searchMovie) => {
            return searchMovie.movieId == movie.movieId
        });
        console.log(existsInHistory);
        this.dialog.open(SelectedMovieDialog, {
            data: {isGuest: isGuest, movie: movie, favoriteMovies: favoriteMovies, isFavorite: isFavorite, updateSearchHistory: !existsInHistory}
        });
    }
}
