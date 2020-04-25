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

    getYears(): number[] {
        var years = [];
        var currentYear = new Date().getFullYear();
        for (var i = currentYear; i > 1900; i--){
            years.push(i);
        }
        return years;
    }

    getYear(year: number): number {
        return year > 0 ? year : null;
    }

    moviesExist(movies: MovieDto[]) : boolean {
        return movies && movies.length > 0; 
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
     * When a user clicks backspace, clear the search results. 
     */
    clearSearchResults() {
        this.movies = null;
        this.noSearchResults = false;
    }

    /**
     * Calls create movie to get the movie a user has selected. 
     * @param imdbId 
     */
    async getSelectedMovie (imdbIdDto: ImdbIdDto) {
        this.toolBarService.isLoading = true;

        await this.createMovie(imdbIdDto)
            .finally(() => this.toolBarService.isLoading = false);

        this.openDialog();
    }

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    openDialog(): void {
        const dialogRef = this.dialog.open(SelectedMovieDialog, {
            width: '450px',
            data: {movieDto: this.selectedMovie}
        });

        dialogRef.afterClosed().subscribe(() => {
            console.log("dialog was closed");
        })
    }

    /**
     * Creates a movie from an ImdbIdDto.
     * @param imdbIdDto 
     */
    private createMovie(imdbIdDto: ImdbIdDto): Promise<any> {
        this.toolBarService.isLoading = true;
        return this.moviesService.createMovieFromImdbId(imdbIdDto).toPromise()
            .then((response) => this.selectedMovie = response)
            .catch((error) => {
                if (error.status == 404) {
                    alert("Error loading movie. Try again later.")
                }
            });
    }
}