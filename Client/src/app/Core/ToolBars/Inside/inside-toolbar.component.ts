import { Component } from '@angular/core';
import { ToolBarService } from '../../Services/tool-bar.service';
import { Router } from '@angular/router';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { SettingsComponent } from 'src/app/Main/Components/Settings/settings.component';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { AfterViewInit } from '@angular/core';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'inside-toolbar',
  templateUrl: './inside-toolbar.component.html',
  styleUrls: ['./inside-toolbar.component.scss']
})
export class InsideToolbarComponent {
    constructor(
        private _toolBarService: ToolBarService, 
        protected _router: Router, 
        private _settingsSheet: MatBottomSheet,
        private dialog: MatDialog)
    {

    }

    // Data
    /**
     * The string being entered into the search bar. Gets sent to server to find movie.
     */
    search: string;
    /**
     * Used to filter search by year.
     */
    year: number;
    /**
     * Holds the array of the movies being shown on current page. Used for client side paging.
     */
    displayedMovies: MovieDto[];
    /**
     * Holds the total number of pages of search results returned by the server.
     */
    totalPages: number;
    /**
     * Used to set the number of movies displayed from search results.
     */
    moviesPerPage: number = 8;
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
     * The movie a user has selected from the search results. 
     */
    selectedMovie: MovieDto;
    /**
     * Used to disable input search when a user is selecting a movie.
     */
    gettingMovie: boolean = false;
    /**
     * 
     */
    searchTableDisplayed: boolean = false;

    // Methods

    /**
     * Uses SwitchMap to to send fromEvent search observable into imdbIds search.
     * Then subscribe and set appropraite variables.
     * ToDo: Look to improve this.
     
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
                        this._toolBarService.isLoading = false;
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
                    this._toolBarService.isLoading = false;
                },
                (error) => {
                    // Handle unauthroized errors with http interceptor.
                    if (error.status != 401) {
                        this.noSearchResults = true;
                        this.movies = null;
                        this.displayedMovies = null;
                        this.totalPages = null;
                        this._toolBarService.isLoading  = false;
                    }
                }
            );
        }
    }*/
    
    /**
     * Returns if the toolbar loading bar should show.
     */
    getIsLoading(): boolean {
        return this._toolBarService.isLoading; 
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
     * Opens settings component. 
     
    openSettings(): void {
        this._settingsSheet.open(SettingsComponent)
    }

      /**
     * Only used to search for imdbIds when a user clicks on magnifying glass OR
     * user selects year to filter search.
     * @param search 
     * @param year 
     
    searchMovies(search: string, year: number) {
        if (search) {
            this._toolBarService.isLoading  = true;
            this.imdbIdsService.getImdbIdsByTitle(search, year) 
                .subscribe((data: MovieDto[]) => {
                    this.noSearchResults = data ? false : true;
                    this.movies = data;
                    if (!this.noSearchResults) {
                        this.setTotalPages(this.movies .length, this.moviesPerPage);
                        this.setDisplayedMovies(this.movies , this.moviesPerPage)
                    }
                    this._toolBarService.isLoading = false;
                })
        }
    }

    /**
     * When a user clicks backspace, clear the search results. 
     
    clearSearchResults(): void {
        this.movies = null;
        this.noSearchResults = false;
    }

    
    /**
     * Gets the year. Checks if greater than 0.
     * @param year 
     
    getYear(year: number): number {
        return year > 0 ? year : null;
    }

    /**
     * Sets the displayed movies shown in the current page. Used for client side paging.
     * @param movies 
     * @param moviesPerPage 
     * @param page 
     
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
     * Gets a movie from imdbId and sets selectedMovie. If that movie does not exist, create the movie
     * and set selectedMovie.
     * @param imdbIdDto 
     
    getOrCreateMovie(imdbIdDto: ImdbIdDto) {
        this.gettingMovie = true;
        this._toolBarService.isLoading = true;
        this._moviesService.$getOrCreateMovie(imdbIdDto.imdbId, imdbIdDto)
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
                    this._toolBarService.isLoading = false;
                },
                () => {
                    this.gettingMovie = false;
                    this._toolBarService.isLoading = false;
                }
            )
    }

    /**
     * Sets the total pages variable. Used for client side paging.
     * @param totalMovies 
     * @param moviesPerPage 
     
    setTotalPages(totalMovies, moviesPerPage): void {
        if (totalMovies > 0) {
            this.totalPages = Math.ceil(totalMovies / moviesPerPage);
        }
    }

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     
    private openDialog(movie, favoriteMovies, isGuest) {
        var isFavorite = this.getIsFavorite(movie, favoriteMovies);
        var existsInHistory = this.searchedMovies.some((searchMovie) => {
            return searchMovie.movieId == movie.movieId
        });
        console.log(existsInHistory);
        this.dialog.open(SelectedMovieDialog, {
            data: {isGuest: isGuest, movie: movie, favoriteMovies: favoriteMovies, isFavorite: isFavorite, updateSearchHistory: !existsInHistory}
        });
    }*/
}
