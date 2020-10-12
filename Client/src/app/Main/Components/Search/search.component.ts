import { HostListener, OnInit } from '@angular/core';
import { AfterViewInit, Component, ViewChild, ElementRef } from "@angular/core";
import { MatBottomSheet, MatDialog } from '@angular/material';
import { Router } from '@angular/router';
import { fromEvent, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap } from 'rxjs/operators';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { ImdbIdsService } from 'src/app/Core/Services/imdbIds.service';
import { MoviesService } from 'src/app/Core/Services/movies.service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { AppUtilities } from 'src/app/Core/Utilities/app-utilities';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { ImdbIdDto } from 'src/app/Data/Interfaces/imdbId.dto';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';

@Component({
    selector: 'search',
    templateUrl: './search.component.html',
    styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit, AfterViewInit {
    constructor(
        private _toolBarService: ToolBarService, 
        protected _router: Router, 
        private _settingsSheet: MatBottomSheet,
        private _imdbIdsService: ImdbIdsService,
        private _moviesService: MoviesService,
        private _favoritesService: FavoritesService,
        private _appUtils: AppUtilities,
        private _dialog: MatDialog)
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
     * 
     */
    imdbs: ImdbIdDto[];
    /**
     * Holds the array of the movies being shown on current page. Used for client side paging.
     */
    displayedImdbs: ImdbIdDto[];
    /**
     * Holds the total number of pages of search results returned by the server.
     */
    totalPages: number;
    /**
     * Used to set the number of movies displayed from search results.
     */
    moviesPerPage: number = 8;
    /**
     * 
     */
    currentPage: number = 0;
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
    /**
     * 
     */
    clickSubscription: Subscription;

    @ViewChild('searchComponent', null) searchComponent: ElementRef;
    @ViewChild('imdbIdSearch', null) imdbIdSearch: ElementRef;
    @HostListener('click', ['$event.target'])
    onClick() {
        if (this.moviesExist(this.imdbs, this.search)) {
            this.searchTableDisplayed = true;
        }
    }
         
    // Methods
    ngOnInit() {
        this.clickSubscription = this._appUtils.clickEvent$.subscribe(
            () => this.searchTableDisplayed = false
        )
    }   

    /**
     * Uses SwitchMap to to send fromEvent search observable into imdbIds search.
     * Then subscribe and set appropraite variables.
     * ToDo: Look to improve this.
     */
    ngAfterViewInit() {
        fromEvent<any>(this.imdbIdSearch.nativeElement, 'keyup') 
        .pipe (
            map((res) => res.target.value),
            debounceTime(1000),
            distinctUntilChanged(),
            switchMap(userSearch => {
                var imdbIdsObservable = this._imdbIdsService.getImdbIdsByTitle(userSearch, this.year);
                // If null, a user has deleted all search chars, and we need to hide loading.
                if (!userSearch) {
                    this._toolBarService.isLoading = false;
                    this.searchTableDisplayed = false;
                }
                return imdbIdsObservable
            })
        )
        .subscribe(
            (data: ImdbIdDto[]) => {
                this.imdbs = data;
                this.setTotalPages(this.imdbs, this.moviesPerPage);
                this.setDisplayedMovies(this.imdbs , this.moviesPerPage, this.currentPage);
                this._toolBarService.isLoading = false;
                this.searchTableDisplayed = true;
            },
            (error) => {
                this.imdbs = null;
                this.displayedImdbs = null;
                this.totalPages = null;
                this._toolBarService.isLoading  = false;
                this.searchTableDisplayed = false;
            }
        );
    }

    /**
     * Only used to search for imdbIds when a user clicks on magnifying glass OR
     * user selects year to filter search.
     * @param search 
     * @param year 
     */
    searchMovies(search: string, year: number) {
        if (search) {
            this._toolBarService.isLoading  = true;
            this._imdbIdsService.getImdbIdsByTitle(search, year) 
                .subscribe(
                    (data: ImdbIdDto[]) => {
                    this.imdbs = data;
                    this.setTotalPages(this.imdbs, this.moviesPerPage);
                    this.setDisplayedMovies(this.imdbs , this.moviesPerPage, this.currentPage)
                    },
                    (error) => alert(error),
                    () => this._toolBarService.isLoading = false
                )
        }
    }

    /**
     * Gets a movie from imdbId and sets selectedMovie. If that movie does not exist, create the movie
     * and set selectedMovie.
     * @param imdbIdDto 
     */
    getOrCreateMovie(imdbIdDto: ImdbIdDto) {
        this.gettingMovie = true;
        this._toolBarService.isLoading = true;
        this._moviesService.$getOrCreateMovie(imdbIdDto.imdbId, imdbIdDto)
            .subscribe(
                (data) => {
                    this.selectedMovie = data; 
                    // Don't open the dialog without a returned movie.
                    if (this.selectedMovie) {
                        this.openDialog(this.selectedMovie);
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
     */
    setTotalPages(imdbs: ImdbIdDto[], moviesPerPage): void {
        if (imdbs != null && imdbs.length > 0) {
            this.totalPages = Math.ceil(imdbs.length / moviesPerPage);
        }
        else {
            this.totalPages = 0;
        }
    }
    
    /**
     * Sets the displayed movies shown in the current page. Used for client side paging.
     * @param movies 
     * @param moviesPerPage 
     * @param page 
     */
    setDisplayedMovies(imdbs:ImdbIdDto[], moviesPerPage:number, page:number): void {
        if (imdbs == null || imdbs.length <= 0) {
            this.displayedImdbs = null; 
        }
        else {
            var startingIndex = page * moviesPerPage; 
            var endingIndex = startingIndex + moviesPerPage;
            imdbs.length > endingIndex 
                ? this.displayedImdbs = imdbs.slice(startingIndex, endingIndex)
                : this.displayedImdbs = imdbs.slice(startingIndex);   
        }
    }

    /**
     * Toggles the movies list and the No Movies Found response in view.
     * @param movies 
     */
    moviesExist(idmbs: ImdbIdDto[], search: string) : boolean {
        if (!search) {
            return false;
        }
        return idmbs && idmbs.length > 0; 
    }

    multiplePagesExist(totalPages: number) {
        return totalPages > 1;
    }

    /**
     * 
     */
    getNextDisplayed(imdbs: ImdbIdDto[], moviesPerPage: number) {
        this.currentPage += 1;
        this.setDisplayedMovies(imdbs, moviesPerPage, this.currentPage);
    }

    /**
     * 
     */
    getPrevDisplayed(imdbs: ImdbIdDto[], moviesPerPage: number) {
        this.currentPage -= 1;
        this.setDisplayedMovies(imdbs, moviesPerPage, this.currentPage);
    }

    /**
     * 
     * @param currentPage 
     * @param totalPages 
     */
    nextPageExists(currentPage: number, totalPages: number) {
        return currentPage < totalPages - 1;
    }

    /**
     * 
     * @param currentPage 
     */
    prevPageExists(currentPage: number) {
        return currentPage > 0;
    }

    /**
     * Gets the year. Checks if greater than 0.
     * @param year 
     */
    getYear(year: number): number {
        return year > 0 ? year : null;
    }

    /**
     * When a user clicks backspace, clear the search results. 
     */
    clearSearchResults(): void {
        this.imdbs = null;
        this.searchTableDisplayed = false;
        this.currentPage = 0;
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
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    private openDialog(movie: MovieDto) {
        this._toolBarService.isLoading = true;

        this._favoritesService.getByMovieId(movie.movieId)
            .subscribe(
                (favorte: FavortiesDto) => {
                    var isFavorite = favorte != null;
                    this._dialog.open(SelectedMovieDialog, {
                        width: '450px',
                        data: {
                            favorite: favorte, 
                            movie: movie, 
                            isFavorite: isFavorite, 
                            updateSearchHistory: false
                        }
                    });
                },
                (error) => alert(error)
            )            
    }
}
