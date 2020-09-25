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
import { clear } from 'console';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, AfterViewInit, OnDestroy {
    constructor(
        private _route: ActivatedRoute,
        private _dialogWatcher: DialogWatcherService,
        private moviesService: MoviesService, 
        private imdbIdsService: ImdbIdsService, 
        private _searchHistoryService: SearchHistoryService,
        private _userService: UserService,
        public toolBarService: ToolBarService, 
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
                else {
                    if (favoritesResolverError.status != 401) {
                        this.error.push(favoritesResolverError);
                        this.error.push(searchHistorResolverError);
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
     * Uses SwitchMap to to send fromEvent search observable into imdbIds search.
     * Then subscribe and set appropraite variables.
     * ToDo: Look to improve this.
     */
    ngAfterViewInit() {
        if (!this.isError(this.error)) {
            fromEvent<any>(this.imdbIdSearch.nativeElement, 'keyup') 
            .pipe (
                map((res) => res.target.value),
                debounceTime(1000),
                distinctUntilChanged(),
                switchMap(userSearch => {
                    var imdbIdsObservable = this.imdbIdsService.getImdbIdsByTitle(userSearch, this.year);
                    // If null, a user has deleted all search chars, and we need to hide loading.
                    if (!userSearch) {
                        this.toolBarService.isLoading = false;
                        this.searchTableDisplayed = false;
                    }
                    return imdbIdsObservable
                })
            )
            .subscribe(
                (data: MovieDto[]) => {
                    this.noSearchResults = data ? false : true;
                    this.movies = data;
                    if (!this.noSearchResults) {
                        this.setTotalPages(this.movies .length, this.moviesPerPage);
                        this.setDisplayedMovies(this.movies , this.moviesPerPage);
                        this.searchTableDisplayed = true;
                    }
                    this.toolBarService.isLoading = false;
                },
                (error) => {
                    // Handle unauthroized errors with http interceptor.
                    if (error.status != 401) {
                        this.noSearchResults = true;
                        this.movies = null;
                        this.displayedMovies = null;
                        this.totalPages = null;
                        this.toolBarService.isLoading  = false;
                    }
                }
            );
        }
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
     * Only used to search for imdbIds when a user clicks on magnifying glass OR
     * user selects year to filter search.
     * @param search 
     * @param year 
     */
    searchMovies(search: string, year: number) {
        if (search) {
            this.toolBarService.isLoading  = true;
            this.imdbIdsService.getImdbIdsByTitle(search, year) 
                .subscribe((data: MovieDto[]) => {
                    this.noSearchResults = data ? false : true;
                    this.movies = data;
                    if (!this.noSearchResults) {
                        this.setTotalPages(this.movies .length, this.moviesPerPage);
                        this.setDisplayedMovies(this.movies , this.moviesPerPage)
                    }
                    this.toolBarService.isLoading = false;
                })
        }
    }

    /**
     * Gets an array of numbers representing the years from 1900 to present.
     */
    getYears(): number[] {
        var years = [];
        var currentYear = new Date().getFullYear();
        for (var i = currentYear; i > 1900; i--){
            years.push(i);
        }
        return years;
    }

    /**
     * Gets the year. Checks if greater than 0.
     * @param year 
     */
    getYear(year: number): number {
        return year > 0 ? year : null;
    }

    /**
     * Toggles the movies list and the No Movies Found response in view.
     * @param movies 
     */
    moviesExist(movies: MovieDto[], search: string) : boolean {
        if (!search) {
            return false;
        }
        return movies && movies.length > 0; 
    }

    /**
     * When a user clicks backspace, clear the search results. 
     */
    clearSearchResults(): void {
        this.movies = null;
        this.noSearchResults = false;
    }
        
    /**
     * Sets the displayed movies shown in the current page. Used for client side paging.
     * @param movies 
     * @param moviesPerPage 
     * @param page 
     */
    setDisplayedMovies(movies:MovieDto[], moviesPerPage:number, page:number=0): void {
        if (movies == null || movies.length <= 0) {
            this.displayedMovies = null; 
        }
        else {
            var startingIndex = page * moviesPerPage; 
            var endingIndex = startingIndex + moviesPerPage;
            movies.length > endingIndex 
                ? this.displayedMovies = movies.slice(startingIndex, endingIndex)
                : this.displayedMovies = movies.slice(startingIndex);   
        }
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
     * Gets a movie from imdbId and sets selectedMovie. If that movie does not exist, create the movie
     * and set selectedMovie.
     * @param imdbIdDto 
     */
    getOrCreateMovie(imdbIdDto: ImdbIdDto) {
        this.gettingMovie = true;
        this.toolBarService.isLoading = true;
        this.moviesService.$getOrCreateMovie(imdbIdDto.imdbId, imdbIdDto)
            .subscribe(
                (data) => {
                    this.selectedMovie = data; 
                    // Don't open the dialog without a returned movie.
                    if (this.selectedMovie) {
                        this.openDialog(this.selectedMovie, this.favorites, this.isGuest);
                    }
                },
                (error) => {
                    if (error.status != 401) {
                        alert("Failed to load movie. Try again later.")
                    }
                    this.gettingMovie = false;
                    this.toolBarService.isLoading = false;
                },
                () => {
                    this.gettingMovie = false;
                    this.toolBarService.isLoading = false;
                }
            )
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
