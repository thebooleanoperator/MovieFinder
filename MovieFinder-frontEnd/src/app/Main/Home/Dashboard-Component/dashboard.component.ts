import { Component } from '@angular/core';
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';
import { ImdbIdDto } from 'src/app/Dto/imdbId.dto';
import { ToolBarService } from 'src/app/Services/tool-bar.service';
import { SelectedMovieDialog } from './Dialogs/selected-movie.dialog';
import { MatDialog } from '@angular/material/dialog';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
    constructor(private moviesService: MoviesService, private toolBarService: ToolBarService, public dialog: MatDialog)
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
     * 
     */
    noSearchResults: boolean = false;
    /**
     * Column titles for search results table.
     */
    public displayedColumns : string[] = ['Title', 'Year'];



    /**
     * Uses a user input search string to return an array of movies.
     * Max 10 movies with names and years displayed to user.
     * @param search 
     */
    searchMovies(search:string) : void {
        // Only search if user such is not null.
        if (search) {
            this.toolBarService.isLoading = true; 
            this.moviesService.getImdbIdsByTitle(search).toPromise()
                .then((response) =>  {
                    this.movies = response
                })
                .catch((error) => {
                    if (error.status == 404) {
                        this.noSearchResults = true;
                    }
                })
                .finally(() => this.toolBarService.isLoading = false);
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
     * Gets a movie by imdb from server. If there is no movie with the imdbId provided,
     * create the movie, then get it. 
     * @param imdbId 
     */
    async getSelectedMovie (imdbIdDto: ImdbIdDto) {
        this.toolBarService.isLoading = true;

        this.selectedMovie = await this.setSelecteMovie(imdbIdDto);
        // If the movie is not in the db, create the movie, then get it.
        if (this.selectedMovie == null) {
            await this.createMovie(imdbIdDto); 
            this.selectedMovie = await this.setSelecteMovie(imdbIdDto);
        }

        this.toolBarService.isLoading = false;
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
     * Gets a movie from server using imdbIdDto.
     * @param imdbId 
     */
    private setSelecteMovie(imdbId: ImdbIdDto): Promise<MovieDto> {
        this.toolBarService.isLoading = true;
        return this.moviesService.getMovieByImdbId(imdbId.imdbId).toPromise()
            .then((response) => response);
    }

    /**
     * Creates a movie from an ImdbIdDto.
     * @param imdbIdDto 
     */
    private createMovie(imdbIdDto: ImdbIdDto): Promise<any> {
        return this.moviesService.createMovieFromImdbId(imdbIdDto).toPromise()
            .then((response) => response);
    }
}