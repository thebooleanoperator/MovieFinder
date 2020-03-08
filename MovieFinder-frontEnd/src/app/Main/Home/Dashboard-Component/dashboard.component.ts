import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';
import { ImdbIdDto } from 'src/app/Dto/imdbId.dto';
import { ToolBarService } from 'src/app/Services/tool-bar.service';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
    /**
     * Holds an array of movies returned from search results.
     */
    movies: Array<MovieDto>;
    /**
     * The movie a user has selected from the search results. 
     */
    selectedMovie: MovieDto;
    /**
     * Column titles for search results table.
     */
    public displayedColumns : string[] = ['Title', 'Year'];

    constructor(private moviesService: MoviesService, private toolBarService: ToolBarService){}

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
                .then((response) =>  this.movies = response)
                .finally(() => this.toolBarService.isLoading = false);
        }
    }

    /**
     * 
     * @param imdbId 
     */
    getSelectedMovie(imdbIdDto: ImdbIdDto) {
        this.toolBarService.isLoading = true;
        this.setSelecteMovie(imdbIdDto)
            .then(() => {
                // If the selected movie is null, we need to add the movie to db.
                if (this.selectedMovie == null) {
                    // Create the movie.
                    this.createMovie(imdbIdDto)
                        .then(() => {
                            // After the movie is created, get the movie info from server.
                            this.setSelecteMovie(imdbIdDto)
                                .finally(() => {
                                    console.log(this.selectedMovie)
                                    this.toolBarService.isLoading = false
                                });
                        }); 
                }   
            })
    }
    
    /**
     * Gets a movie from server using imdbIdDto.
     * @param imdbId 
     */
    private setSelecteMovie(imdbId: ImdbIdDto): Promise<any> {
        return this.moviesService.getMovieByImdbId(imdbId.imdbId).toPromise()
            .then((response) => {
                console.log("movie", response);
                this.selectedMovie = response
            }); 
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