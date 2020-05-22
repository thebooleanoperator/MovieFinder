import { Component, ViewChild, OnInit } from '@angular/core';
import { MovieDto } from 'src/app/Data/movie.dto';
import { MoviesService } from 'src/app/Core/Services/movies.service';
import { ImdbIdDto } from 'src/app/Data/imdbId.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { MatDialog } from '@angular/material/dialog';
import { ImdbIdsService } from 'src/app/Core/Services/imdbIds.service';
import { ActivatedRoute } from '@angular/router';
import { FavortiesDto } from 'src/app/Data/favorites.dto';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';
import { Observable, fromEvent } from 'rxjs';
import { map, startWith, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    constructor(
        private _route: ActivatedRoute,
        private _dialogWatcher: DialogWatcherService,
        private moviesService: MoviesService, 
        private imdbIdsService: ImdbIdsService, 
        private toolBarService: ToolBarService, 
        private dialog: MatDialog){}

    /**
     * Holds an array of all movies returned from search results.
     */
    movies: MovieDto[];
    /**
     * 
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
     * True if a catch block in a promise has been entered. 
     * Don't want to show multiple alerts to user for 1 request.
     */
    errorFound: boolean = false;

    ngOnInit() {
        this._route.data.subscribe((data) => {
            this.favorites = data.favorites;
        })
    }

    /**
     * Uses SwitchMap to to send fromEvent search observable into imdbIds search.
     * Then subscribe and set appropraite variables.
     * ToDo: Look to improve this.
     */
    $searchForImdbIds = 
        fromEvent<any>(document, 'keyup') 
            .pipe (
                map((event) => event.target.value),
                debounceTime(1000),
                distinctUntilChanged(),
                switchMap(userSearch => {
                    var imdbIdsObservable = this.imdbIdsService.getImdbIdsByTitle(userSearch, this.year);
                    // If null, a user has deleted all search chars, and we need to hide loading.
                    if (!userSearch) {
                        this.toolBarService.isLoading = false;
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
                        this.setDisplayedMovies(this.movies , this.moviesPerPage)
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
                    if (this.selectedMovie) {
                        this.openDialog(this.selectedMovie, this.favorites);
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
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    private openDialog(movie, favoriteMovies) {
        var isFavorite = this.getIsFavorite(movie, favoriteMovies);
        this.dialog.open(SelectedMovieDialog, {
            width: '450px',
            data: {movie: movie, favoriteMovies: favoriteMovies, isFavorite: isFavorite}
        });

        this._dialogWatcher.$closeEvent.subscribe((favorites) => {
            this.favorites = favorites;
        })
    }
}