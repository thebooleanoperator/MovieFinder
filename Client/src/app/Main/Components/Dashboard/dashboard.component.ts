import { Component, ViewChild, OnInit } from '@angular/core';
import { MovieDto } from 'src/app/Data/movie.dto';
import { MoviesService } from 'src/app/Core/Services/movies.service';
import { ImdbIdDto } from 'src/app/Data/imdbId.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { MatDialog } from '@angular/material/dialog';
import { ImdbIdsService } from 'src/app/Core/Services/imdbIds.service';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
    constructor(private moviesService: MoviesService, private imdbIdsService: ImdbIdsService, 
        private toolBarService: ToolBarService, public dialog: MatDialog){}

    /**
     * Holds an array of all movies returned from search results.
     */
    movies: MovieDto[];
    /**
     * 
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
     * 
     */
    totalPages: number;
    
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
     * Uses a user input search string to return an array of movies.
     * @param search 
     */
    searchMovies(search:string, year:number) : void {
        if (this.timeout) {
            clearTimeout(this.timeout);
            this.movies = null;
        }
        // Only search if user search is not null.
        if (search) {
            this.timeout = setTimeout(() => {
                this.toolBarService.isLoading = true;
                this.imdbIdsService.getImdbIdsByTitle(search, year).toPromise()
                    .then((response) =>  {
                        this.movies = response;
                        this.setTotalPages(this.movies.length, this.moviesPerPage);
                        this.setDisplayedMovies(this.movies, this.moviesPerPage)
                        this.noSearchResults = this.movies.length > 0 ? false : true;
                    })
                    .catch((error) => {
                        // Clear search results and show not found message on 404 and 500.
                        if (error.status == 404 || error.status == 500) {
                            this.noSearchResults = true;
                            this.movies = null;
                            this.displayedMovies = null;
                            this.totalPages = null;
                        }
                    })
                    .finally(() => this.toolBarService.isLoading = false);
            }, 700);
        }
    }

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

    setTotalPages(totalMovies, moviesPerPage): void {
        if (totalMovies > 0) {
            this.totalPages = Math.ceil(totalMovies / moviesPerPage);
        }
    }

    /**
     * Gets a movie from imdbId and sets selectedMovie. If that movie does not exist, create the movie
     * and set selectedMovie.
     * @param imdbIdDto 
     */
    async getOrCreateMovie(imdbIdDto: ImdbIdDto) {
        this.gettingMovie = true;
        this.selectedMovie = await this.getMovie(imdbIdDto.imdbId); 

        if (!this.selectedMovie) {
            this.selectedMovie = await this.createMovie(imdbIdDto); 
        }
        this.gettingMovie = false;
        // Only open the dialog if selectedMovie is set.
        if (this.selectedMovie) {
            this.openDialog();
        }
    }

    /**
     * Aysnchronous function that gets a movie from MovieFinderAPI with imdbId string.
     * @param imdbId 
     */
    private async getMovie(imdbId: string): Promise<MovieDto> {
        this.toolBarService.isLoading = true;
        return await this.moviesService.getMovieByImdbId(imdbId).toPromise()
            .then((movieDto) => movieDto)
            .catch((error) => {
                if (error.status == 400) {
                    alert("Cannot find movie.")
                }
            })
            .finally(() =>this.toolBarService.isLoading = false);
    }

    /**
     * Aysnchronous function that creates a movie with MovieFinderAPI.
     * Sends an ImdbIdDto as a param to api.
     * @param imdbId 
     */
    private async createMovie(imdbIdDto: ImdbIdDto): Promise<MovieDto> {
        this.toolBarService.isLoading = true;
        return await this.moviesService.createMovieFromImdbId(imdbIdDto).toPromise()
            .then((moviesDto) => moviesDto)
            .catch((error) => {
                if (error.status == 404) {
                    alert("Movie not found");
                }
            })
            .finally(() =>this.toolBarService.isLoading = false);
    }

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    private openDialog() {
        const dialogRef = this.dialog.open(SelectedMovieDialog, {
            width: '450px',
            data: {movieDto: this.selectedMovie}
        });

        dialogRef.afterClosed().subscribe(() => {
            console.log("dialog was closed");
        });
    }
}