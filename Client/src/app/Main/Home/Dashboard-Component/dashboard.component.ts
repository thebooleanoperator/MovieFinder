import { Component, OnInit } from '@angular/core';
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';
import { ImdbIdDto } from 'src/app/Dto/imdbId.dto';
import { ToolBarService } from 'src/app/Services/tool-bar.service';
import { SelectedMovieDialog } from './Dialogs/selected-movie.dialog';
import { MatDialog } from '@angular/material/dialog';
import { ImdbIdsService } from 'src/app/Services/imdbIds.service';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
    constructor(private moviesService: MoviesService, private imdbIdsService: ImdbIdsService, private toolBarService: ToolBarService, public dialog: MatDialog)
    {

    }
    
    /**
     * Holds an array of movies returned from search results.
     */
    movies: Array<MovieDto>;
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
    moviesExist(movies: MovieDto[]) : boolean {
        return movies && movies.length > 0; 
    }

    /**
     * When a user clicks backspace, clear the search results. 
     */
    clearSearchResults() {
        this.movies = null;
        this.noSearchResults = false;
    }

    /**
     * Uses a user input search string to return an array of movies.
     * Max 10 movies with names and years displayed to user.
     * @param search 
     */
    searchMovies(search:string, year:number) : void {
        if (this.timeout) {
            clearTimeout(this.timeout);
        }
        // Only search if user such is not null.
        if (search) {
            this.timeout = setTimeout(() => {
            this.toolBarService.isLoading = true;
                this.imdbIdsService.getImdbIdsByTitle(search, year).toPromise()
                    .then((response) =>  {
                        this.movies = response;
                        this.noSearchResults = this.movies.length > 0 ? false : true; 
                    })
                    .catch((error) => {
                        // Clear search results and show not found message on 404 and 500.
                        if (error.status == 404 || error.status == 500) {
                            this.noSearchResults = true;
                            this.movies = null;
                        }
                    })
                    .finally(() => this.toolBarService.isLoading = false);
            }, 700);
        }
    }

    /**
     * Gets a movie from imdbId and sets selectedMovie. If that movie does not exist, create the movie
     * and set selectedMovie.
     * @param imdbIdDto 
     */
    async getOrCreateMovie(imdbIdDto: ImdbIdDto) {
        this.selectedMovie = await this.getMovie(imdbIdDto.imdbId); 

        if (!this.selectedMovie) {
            this.selectedMovie = await this.createMovie(imdbIdDto); 
        }
        // Only open the dialog if seletedMovie is set.
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
        })
    }
}